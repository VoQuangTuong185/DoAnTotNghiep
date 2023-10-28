import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { Subscription, finalize } from 'rxjs';
import { AuthService } from '../core/src/lib/Auth.service';
import { LoginService } from '../data/login.service';
import { AddressService } from '../data/Address.service';
import { User } from '../data/User.model';
import { WebsiteAPIService } from '../data/WebsiteAPI.service'
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  displayRegisterPopUp: boolean | undefined = true;
  displayConfirmPopUp: boolean | undefined = false;
  hide = true;
  registerForm: FormGroup;
  registerSuccessfully:boolean = false;
  firstLoad:boolean = true;
  confirmCodeSent :string ='';
  confirmCode :string ='';
  confirmEmail : string ='';
  provices: any[];
  districts: any[];
  wards: any[];
  provinceSelected: any;
  districtSelected: any;
  wardSelected: any;
  constructor(
    public dialog: MatDialog, 
    private primengConfig: PrimeNGConfig, 
    private formBuilder : FormBuilder, 
    private websiteAPIService : WebsiteAPIService,
    private messageService: MessageService,
    private router: Router,
    private authService: AuthService,
    private addressService: AddressService
    ) {
      this.registerForm = this.createEmptyRegisterForm();
      this.provices = [];
      this.districts = [];
      this.wards = [];
      this.getAllProvices();
    }
    createEmptyRegisterForm() {
      return this.formBuilder.group({
        Name: ['',Validators.required],
        LoginName: ['',Validators.required],
        Email: ['',Validators.required],
        Provinces: ['',Validators.required],
        Districts: ['',Validators.required],
        Wards: ['',Validators.required],
        ProvinceCode: ['',Validators.required],
        DistrictCode: ['',Validators.required],
        WardCode: ['',Validators.required],
        Address: ['',Validators.required],
        TelNum: ['',Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])],
        Password: ['',Validators.required],
      })
    }
    createExistedregisterForm() {
      return this.formBuilder.group({
        Name: ['',Validators.required],
        LoginName: ['',Validators.required],
        Email: ['',Validators.required],
        Provinces: [this.provices,Validators.required],
        Districts: [this.districts,Validators.required],
        Wards: [this.wards,Validators.required],
        ProvinceCode: [this.provinceSelected.code,Validators.required],
        DistrictCode: [this.districtSelected.code,Validators.required],
        WardCode: [this.wardSelected.code,Validators.required],
        Address: ['',Validators.required],
        TelNum: ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])],
        Password: ['',Validators.required],
      })
    }
  ngOnInit() {
    this.primengConfig.ripple = true;
    this.registerForm.get('Provinces')?.valueChanges.subscribe(provices => {
      this.provinceSelected = provices;
      this.getDistrictsOfProvice();
    });
    this.registerForm.get('Districts')?.valueChanges.subscribe(districts => {
      this.districtSelected = districts;
      this.getWardsOfDistrict();
    });
    this.registerForm.get('Wards')?.valueChanges.subscribe(userWard => {
      this.wardSelected = userWard
    });
    if (this.provices.length > 0) {
      this.registerForm = this.createExistedregisterForm();
    }
  }
  getAllProvices(): void {
    this.addressService.getProvices().pipe().subscribe(data =>this.provices = data);
  }
  getDistrictsOfProvice(): void {
    this.wards = [];
    this.addressService.getDistrictsOfProvince(this.provinceSelected.code).subscribe(data => this.districts = data.districts)
  }
  getWardsOfDistrict(): void {
    this.addressService.getWardsOfDistrict(this.districtSelected.code).subscribe(data => this.wards = data.wards)
  }
  hideBasicDialog() {
    this.displayRegisterPopUp = false;
  }
  register() { 
    this.registerForm.controls['ProvinceCode'].setValue(this.registerForm.get('Provinces')?.value.code);
    this.registerForm.controls['DistrictCode'].setValue(this.registerForm.get('Districts')?.value.code);
    this.registerForm.controls['WardCode'].setValue(this.registerForm.get('Wards')?.value.code);
    this.registerForm.controls['Provinces'].setValue(this.provinceSelected.name);
    this.registerForm.controls['Districts'].setValue(this.districtSelected.name);
    this.registerForm.controls['Wards'].setValue(this.wardSelected.name);
    if(this.registerForm.invalid){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập các thông tin bắt buộc để đăng ký!',
      });
      return;
    }
    this.websiteAPIService.checkExistedAndSendConfirmMail(this.registerForm.getRawValue()).subscribe((res:any) =>{      
      if(res.data != 'existed'){
        this.confirmEmail = this.registerForm.getRawValue().Email;
        this.displayConfirmPopUp = true;
        this.displayRegisterPopUp = false;
        this.confirmCodeSent = res.data;
        this.messageService.add({key: 'bc', severity:'info', summary: 'Thông tin', detail: 'Mã xác nhận đăng ký đã được gửi đi!'});
      }
      else{
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Đã tồn tại tài khoản hoặc địa chỉ email !'});
      }
    });
  }
  confirmRegister() {
    this.firstLoad = false; 
    if(this.confirmCode == this.confirmCodeSent){
      this.authService.register(this.registerForm.getRawValue()).subscribe((res:any) =>{
        if(res.data){
          this.messageService.add({key: 'c', sticky: true, severity:'success', summary:'Xác nhận?', detail:'Quay về trang đăng nhập'});
        }
      });
    }
    else{
      this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Xác nhận không thành công, hãy thử lại!'});
    }
  }
  onConfirmLoginPage() {
    this.messageService.clear();
    this.router.navigate(['login-user']);
  }
  onRejectLoginPage() {
    this.messageService.clear();
  }
}
