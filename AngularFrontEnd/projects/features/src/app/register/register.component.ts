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
  loginForm: FormGroup;
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
      this.loginForm = this.createEmptyLoginForm();
      this.provices = [];
      this.districts = [];
      this.wards = [];
      this.getAllProvices();
    }
    createEmptyLoginForm() {
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
        TelNum: ['',Validators.maxLength(10)],
        Password: ['',Validators.required],
      })
    }
    createExistedLoginForm() {
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
        TelNum: ['',Validators.maxLength(10)],
        Password: ['',Validators.required],
      })
    }
  ngOnInit() {
    this.primengConfig.ripple = true;
    this.loginForm.get('Provinces')?.valueChanges.subscribe(provices => {
      this.provinceSelected = provices;
      this.getDistrictsOfProvice();
    });
    this.loginForm.get('Districts')?.valueChanges.subscribe(districts => {
      this.districtSelected = districts;
      this.getWardsOfDistrict();
    });
    this.loginForm.get('Wards')?.valueChanges.subscribe(userWard => {
      this.wardSelected = userWard
    });
    if (this.provices.length > 0) {
      this.loginForm = this.createExistedLoginForm();
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
    this.loginForm.controls['ProvinceCode'].setValue(this.loginForm.get('Provinces')?.value.code);
    this.loginForm.controls['DistrictCode'].setValue(this.loginForm.get('Districts')?.value.code);
    this.loginForm.controls['WardCode'].setValue(this.loginForm.get('Wards')?.value.code);
    this.loginForm.controls['Provinces'].setValue(this.provinceSelected.name);
    this.loginForm.controls['Districts'].setValue(this.districtSelected.name);
    this.loginForm.controls['Wards'].setValue(this.wardSelected.name);
    this.websiteAPIService.checkExistedAndSendConfirmMail(this.loginForm.getRawValue()).subscribe((res:any) =>{      
      if(res.data != 'existed'){
        this.confirmEmail = this.loginForm.getRawValue().Email;
        this.displayConfirmPopUp = true;
        this.displayRegisterPopUp = false;
        this.confirmCodeSent = res.data;
        this.messageService.add({key: 'bc', severity:'info', summary: 'Info', detail: 'Confirm code was sent!'});
      }
      else{
        this.messageService.add({key: 'bc', severity:'error', summary: 'Error', detail: 'Existed Login Name or Email, try again!'});
      }
    });
  }
  confirmRegister() {
    this.firstLoad = false; 
    if(this.confirmCode == this.confirmCodeSent){
      this.authService.register(this.loginForm.getRawValue()).subscribe((res:any) =>{
        if(res.data){
          this.messageService.add({key: 'c', sticky: true, severity:'success', summary:'Are you sure?', detail:'Go to login page now'});
        }
      });
    }
    else{
      this.messageService.add({key: 'bc', severity:'error', summary: 'Error', detail: 'Wrong code, try again!'});
    }
  }
  onConfirmLoginPage() {
    this.messageService.clear();
    this.router.navigate(['/user/login-user']);
  }
  onRejectLoginPage() {
    this.messageService.clear();
  }
}
