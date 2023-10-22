import { Component, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService, PrimeNGConfig } from 'primeng/api';
import { Table } from 'primeng/table';
import { AdminUserDTO } from '../data/AdminUserDTO';
import { WebsiteAPIService } from '../data/WebsiteAPI.service'
import { User } from '../data/User.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-admin-user',
  templateUrl: './admin-user.component.html',
  styleUrls: ['./admin-user.component.scss'],
})
export class AdminUserComponent {
  dataUsers!: AdminUserDTO[];
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
        { header : 'Họ và tên', field : 'name', width:15, type:'string'},
        { header : 'Tài khoản', field : 'loginName', width:15, type:'string'},
        { header : 'Địa chỉ email', field : 'email', width:20, type:'string'},
        { header : 'Địa chỉ', field : 'address', width:20, type:'string'},
        { header : 'Số điện thoại', field : 'telNum', width:10, type:'string'},
        { header : 'Trạng thái', field : 'isActive', width:5, type:'boolean'},
        { header : 'Thao tác', field : 'action', width:20, type:'button'},
    ];
    if(localStorage.getItem('authToken') != 'null'){
      this.userData = JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]));
    }  
  }
  createEmptyUserForm(){
    return this.formBuilder.group({
      Name: ["",[Validators.required]],
      LoginName: ["",[Validators.required]],
      Email: ["",[Validators.required]],
      Tel: ["",[Validators.maxLength(10)]]
    })
  }
  createExistedUserForm(){
    return this.formBuilder.group({
      Id: [this.selectedUsers.id,[Validators.required]],
      Name: [this.selectedUsers.name,[Validators.required]],
      LoginName: [this.selectedUsers.loginName,[Validators.required]],
      Email: [{value: this.selectedUsers.email, disabled: true},[Validators.required]],
      TelNum: [this.selectedUsers.telNum,[Validators.maxLength(10)]]
    })
  }
  loadDataUsers(){
    this.websiteAPIService.getUsers().subscribe((res:any) => {
      this.dataUsers = res.data; 
      this.dataForCheckingExisted = res.data;
      this.loading = false;
    });
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
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: data.name + ' was ' + info, life: 3000});
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
    this.websiteAPIService.editUser(this.editUserForm.getRawValue()).subscribe((res:any) =>{
      if(res.data){
        this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message});
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
}
