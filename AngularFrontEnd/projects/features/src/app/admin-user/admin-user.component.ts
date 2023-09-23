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
        { header : 'Id', field : 'id', width:5, type:'number'},
        { header : 'Name', field : 'name', width:15, type:'string'},
        { header : 'Login Name', field : 'loginName', width:15, type:'string'},
        { header : 'Email', field : 'email', width:20, type:'string'},
        { header : 'Address', field : 'address', width:20, type:'string'},
        { header : 'Telephone', field : 'telNum', width:10, type:'string'},
        { header : 'Active', field : 'isActive', width:5, type:'boolean'},
        { header : 'Action', field : 'action', width:20, type:'button'},
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
      this.messageService.add({key: 'bc', severity:'info', summary: 'Info', detail: 'You can not inactive yourself!', life: 3000});
      return;
    }
    var info = '';
    info = data.IsActive ? 'InActive' : 'Active';
    this.confirmationService.confirm({
      message: 'Are you sure '+ info+ ' user: ' + data.name + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.websiteAPIService.activeOrInActiveUser(data.loginName).subscribe((res:any) =>{
          if(res.data){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Successful', detail: data.name + ' was ' + info, life: 3000});
            this.loadDataUsers();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Error', detail: info + data.name + ' fail, try again!'});
          }
        });         
      }
    });
  }
  confirmEditUser(){
    this.websiteAPIService.editUser(this.editUserForm.getRawValue()).subscribe((res:any) =>{
      if(res.data){
        this.messageService.add({key: 'bc', severity:'success', summary: 'Successful', detail: res.message});
        this.loadDataUsers();
      }
      else {
        this.messageService.add({key: 'bc', severity:'error', summary: 'Error', detail: res.message});
      }
    });
  }
  setManagerPermisson(data:any){    
    var info = '';
    this.selectedUsers = {...data};
    if (this.userData.id == this.selectedUsers.id){
      this.messageService.add({key: 'bc', severity:'info', summary: 'Info', detail: 'You can not modify yourself!', life: 3000});
      return;
    }
    this.selectedUsers.userAPIs.forEach(e => { 
      e.roleId === 2 && e.isActive ? info = 'remove manager permission' : info = 'set manager permission';
    })
    this.currentUserId = this.selectedUsers.id;
    this.confirmationService.confirm({
      message: 'Are you sure '+info+' for user: ' + this.selectedUsers.name + '?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.websiteAPIService.setManagerPermisson(this.currentUserId).subscribe((res:any) =>{
          if(res.data){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Successful', detail: res.message, life: 3000});
            this.loadDataUsers();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Error', detail: res.message});
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
      e.roleId === 2 && e.isActive ? tooltip = 'remove manager permission' : tooltip = 'set manager permission';
    })
    return tooltip;
  }
}
