import { Component, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService, PrimeNGConfig } from 'primeng/api';
import { Table } from 'primeng/table';
import { AdminUserDTO } from '../data/AdminUserDTO';
import { WebsiteAPIService } from '../data/WebsiteAPI.service'
import { User } from '../data/User.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import jwt_decode, { JwtPayload } from 'jwt-decode'
import { Event } from '@angular/router';

@Component({
  selector: 'app-admin-user',
  templateUrl: './admin-user.component.html',
  styleUrls: ['./admin-user.component.scss'],
})
export class AdminUserComponent {
  dataUsers!: AdminUserDTO[];
  originalDataUsers!: AdminUserDTO[];
  usersDataCols: any[] = [];
  selectedUsers!: AdminUserDTO;
  dataForCheckingExisted!: AdminUserDTO[];
  first:number = 10;
  rows:number = 10;
  statuses!: any[];
  searchText: string ='';
  loading: boolean = true;
  displayEditUserPopup :boolean = false;
  submitted: boolean = false;
  userData = new User();  
  editUserForm!: FormGroup;
  currentUserId!: number;
  roles!: any[];
  selectedRole: any = 'all';
  selectedRoleIndex: number = 0;
  @ViewChild('dt') table!: Table;

  constructor(
    private websiteAPIService : WebsiteAPIService, 
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private formBuilder : FormBuilder)
  { 
    this.editUserForm = this.createEmptyUserForm();
    this.usersDataCols = [
        { header : 'STT', field : 'id', width:5, type:'number'},
        { header : 'Họ và tên', field : 'name', width:18, type:'string'},
        { header : 'Tài khoản', field : 'loginName', width:10, type:'string'},
        { header : 'Giảm giá VIP', field : 'discount', width:5, type:'percent'},
        { header : 'Địa chỉ email', field : 'email', width:15, type:'string'},
        { header : 'Địa chỉ', field : 'address', width:22, type:'string'},
        { header : 'Số điện thoại', field : 'telNum', width:8, type:'string'},
        { header : 'Trạng thái', field : 'isActive', width:7, type:'boolean'},
        { header : 'Thao tác', field : 'action', width:15, type:'button'},
    ];
    if(localStorage.getItem('authToken') != 'null'){
      this.userData = jwt_decode(localStorage.getItem('authToken')!.replace(/-/g, "+").replace(/_/g, "/"));
    }  
    this.roles = [
      { name: 'Tất cả',  value: 'all'},
      { name: 'Quản trị viên',  value: 'admin'},
      { name: 'Khách hàng',  value: 'customer' }
  ];
  }
  createEmptyUserForm(){
    return this.formBuilder.group({
      Name: ["",[Validators.required,Validators.maxLength(50)]],
      LoginName: ["",[Validators.required, Validators.maxLength(50)]],
      Email: ["",Validators.compose([Validators.required, Validators.email, Validators.maxLength(255)])],
      Tel: ["",Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])]
    })
  }
  createExistedUserForm(){
    return this.formBuilder.group({
      Id: [this.selectedUsers.id,[Validators.required]],
      Name: [this.selectedUsers.name,[Validators.required,Validators.maxLength(50)]],
      LoginName: [this.selectedUsers.loginName,[Validators.required, Validators.maxLength(50)]],
      Email: [{value: this.selectedUsers.email, disabled: true},Validators.compose([Validators.required, Validators.email, Validators.maxLength(255)])],
      TelNum: [this.selectedUsers.telNum,Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])]
    })
  }
  loadDataUsers(){
    this.websiteAPIService.getUsers().subscribe((res:any) => {
      this.dataUsers = res.data;
      this.originalDataUsers = res.data; 
      this.dataForCheckingExisted = res.data;
      this.loading = false;
    });
    this.selectedRole = 'all';
  }
  ngOnInit() {
    this.loadDataUsers();
  }
  editUser(data:any){    
    this.selectedUsers = {...data};
    this.editUserForm = this.createExistedUserForm();
    this.displayEditUserPopup = true;
  }
  hideEditUserPopup() {
    this.displayEditUserPopup = false;
    this.submitted = false;
  }
  activeOrInActiveUser(data:any){
    if (this.userData.id == data.id){
      this.messageService.add({key: 'bc', severity:'info', summary: 'Thông tin', detail: 'Bạn không thể khoá tài khoản của chính bạn!', life: 3000});
      return;
    }
    var info = '';
    info = data.isActive == true ? 'khoá' : 'mở khoá';
    this.confirmationService.confirm({
      message: 'Xác nhận '+ info+ ' tài khoản: ' + data.name + '?',
      header: 'Xác nhận',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.websiteAPIService.activeOrInActiveUser(data.loginName).subscribe((res:any) =>{
          if(res.data){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: 'Tài khoản của ' + data.name + ' đã ' + info, life: 3000});
            this.loadDataUsers();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: info + data.name + ' lỗi, hãy thử lại!'});
          }
        });         
      }
    });
  }
  confirmEditUser(){
    if(this.editUserForm.invalid){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập các thông tin bắt buộc!',
      });
      return;
    }
    this.websiteAPIService.editUser(this.editUserForm.getRawValue()).subscribe((res:any) =>{
      if(res.data){
        this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message});
        this.displayEditUserPopup = false;
        this.loadDataUsers();
      }
      else {
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
      }
    });
  }
  setManagerPermisson(data:any){    
    var info = '';
    this.selectedUsers = {...data};
    if (this.userData.id == this.selectedUsers.id){
      this.messageService.add({key: 'bc', severity:'info', summary: 'Thông tin', detail: 'Bạn không thể thay đổi quyền cho chính bạn!', life: 3000});
      return;
    }
    this.selectedUsers.userAPIs.forEach(e => { 
      e.roleId === 2 && e.isActive ? info = 'thu hồi quyền quản trị' : info = 'thêm quyền quản trị';
    })
    this.currentUserId = this.selectedUsers.id;
    this.confirmationService.confirm({
      message: 'Xác nhận '+info+' cho tài khoản: ' + this.selectedUsers.name + '?',
      header: 'Xác nhận',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.websiteAPIService.setManagerPermisson(this.currentUserId).subscribe((res:any) =>{
          if(res.data){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
            this.loadDataUsers();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
          }
        });         
      }
    });
  }
  handleButtonSetManager(data: any){
    var isAdmin = false;
    this.selectedUsers = {...data};
    this.selectedUsers.userAPIs.forEach(e => {
      if(e.roleId === 2 && e.isActive){
        isAdmin = true;
      }
    });
    return isAdmin;
  }
  handleTooltipSetManager(data : any){
    var tooltip = '';
    this.selectedUsers = {...data};
    this.selectedUsers.userAPIs.forEach(e => {
      e.roleId === 2 && e.isActive ? tooltip = 'thu hồi quyền quản trị' : tooltip = 'thêm quyền quản trị';
    })
    return tooltip;
  }
  changeRoleUserDisplay(value : any){
    this.selectedRoleIndex = value.index;
    switch(value.index) { 
      case 1: { 
        this.dataUsers = this.originalDataUsers.filter(x => this.handleButtonSetManager(x) == true)
        break; 
      } 
      case 2: { 
        this.dataUsers = this.originalDataUsers.filter(x => this.handleButtonSetManager(x) == false)
        break; 
      } 
      default: { 
        this.dataUsers = this.originalDataUsers; 
        break; 
      } 
   }     
  }
}
