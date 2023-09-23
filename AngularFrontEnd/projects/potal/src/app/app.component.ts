import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Subscription, finalize } from 'rxjs';
import { AuthService } from '../../../features/src/app/core/src/lib/Auth.service';
import { UserProfile } from '../../../features/src/app/data/UserProfile.model';
import { LoginService } from '../../../features/src/app/data/login.service';
import { User } from '../../../features/src/app/data/User.model';
import { UserRole } from '../../../features/src/app/data/UserRole.model';
import { WebsiteAPIService } from '../../../features/src/app/data/WebsiteAPI.service';
import { AddressService } from '../../../features/src/app/data/Address.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'Website API ^^';
  currentTab!: string;
  isAdmin: boolean = false;
  role!: string;
  FirstLoad: boolean = true;
  subscription: Subscription;
  loginName: string = '';
  userData = new User();
  selectedUsers!: UserProfile;
  editUserForm!: FormGroup;
  displayEditUserPopup: boolean = false;
  submitted: boolean = false;
  displayChangeMailPopup: boolean = false;
  confirmEmail: string = '';
  confirmCode: string = '';
  confirmCodeSent: string = '';
  IsReceiveConfirmCode: boolean = false;
  IsCorrectCode: boolean = false;
  provices: any[];
  districts: any[];
  wards: any[];
  provinceSelected: any;
  districtSelected: any;
  wardSelected: any;
  existedProvince: any;
  existedDistrict: any;
  existedWard: any;
  constructor(
    private router: Router,
    private loginService: LoginService,
    private confirmationService: ConfirmationService,
    private authService: AuthService,
    private messageService: MessageService,
    private websiteAPIService: WebsiteAPIService,
    private formBuilder: FormBuilder,
    private addressService: AddressService
  ) {
    this.provices = [];
    this.districts = [];
    this.wards = [];
    this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
        this.currentTab = this.router.url;
      }
    });
    if (
      localStorage.getItem('authToken') != null &&
      localStorage.getItem('authToken') != 'null'
    ) {
      this.userData = JSON.parse(
        window.atob(localStorage.getItem('authToken')!.split('.')[1])
      );
      this.authService
        .checkValidToken(this.userData.loginName)
        .subscribe((res: any) => {
          if (res.isSuccess) {
            this.userData.isLoggedIn = true;
            this.isAdmin = this.userData.role === UserRole.Admin;
            this.isAdmin
              ? (this.role = UserRole.Admin)
              : (this.role = UserRole.User);
            localStorage.setItem('authToken', res.data);
          } else {
            localStorage.removeItem('authToken');
            this.userData.isLoggedIn = false;
            this.messageService.add({
              key: 'bc',
              severity: 'info',
              summary: 'Info',
              detail: res.data,
            });
            this.router.navigate(['/user/login-user']);
          }
        });
    } else {
      this.router.navigate(['']);
    }
    this.subscription = loginService.userLogin$.subscribe((mission) => {
      this.userData = mission;
      if (this.userData.isLoggedIn) {
        this.checkUser();
      }
    });
  }
  checkUser() {
    this.loginName = this.userData.loginName;
    this.isAdmin = this.userData.role === UserRole.Admin;
    this.isAdmin ? (this.role = UserRole.Admin) : (this.role = UserRole.User);
  }
  ngOnInit() {
    this.getAllProvices();
    this.loadDataUser();
    this.checkUser();
    if (
      localStorage.getItem('provinces') != null &&
      localStorage.getItem('provinces') != 'null'
    ) {
      this.provices = JSON.parse(
        window.atob(localStorage.getItem('provinces')!)
      );
    }
    if (
      localStorage.getItem('districts') != null &&
      localStorage.getItem('districts') != 'null'
    ) {
      this.districts = JSON.parse(
        window.atob(localStorage.getItem('districts')!)
      );
    }
    if (
      localStorage.getItem('wards') != null &&
      localStorage.getItem('wards') != 'null'
    ) {
      this.wards = JSON.parse(window.atob(localStorage.getItem('wards')!));
    }
  }
  createExistedUserForm() {
    return this.formBuilder.group({
      Id: [this.selectedUsers.id, [Validators.required]],
      Name: [this.selectedUsers.name, [Validators.required]],
      LoginName: [this.selectedUsers.loginName, [Validators.required]],
      Email: [this.selectedUsers.email, [Validators.required]],
      TelNum: [this.selectedUsers.telNum, [Validators.maxLength(10)]],
      Provinces: [this.existedProvince[0], Validators.required],
      Districts: [this.existedDistrict[0], Validators.required],
      Wards: [this.existedWard[0], Validators.required],
      ProvinceCode: [this.selectedUsers.provinceCode, Validators.required],
      DistrictCode: [this.selectedUsers.districtCode, Validators.required],
      WardCode: [this.selectedUsers.wardCode, Validators.required],
      Streets: [this.selectedUsers.streets, Validators.required],
    });
  }
  loadDataUser() {
    this.websiteAPIService
      .getInfoUser(this.userData.id)
      .subscribe((res: any) => {
        if (res.isSuccess) {
          this.selectedUsers = res.data;
          this.getDistrictsOfProvice(
            this.selectedUsers.provinceCode,
            this.selectedUsers.districtCode
          );
          this.getWardsOfDistrict(
            this.selectedUsers.districtCode,
            this.selectedUsers.wardCode
          );
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'info',
            summary: 'Info',
            detail: res.message,
          });
        }
      });
  }
  editUser() {
    this.existedProvince = this.provices.filter(
      (x) => x.code == this.selectedUsers.provinceCode
    );
    this.provinceSelected = this.existedProvince[0];

    this.existedDistrict = this.districts.filter(
      (x) => x.code == this.selectedUsers.districtCode
    );
    this.districtSelected = this.existedDistrict[0];

    this.existedWard = this.wards.filter(
      (x) => x.code == this.selectedUsers.wardCode
    );
    this.wardSelected = this.existedWard[0];

    this.editUserForm = this.createExistedUserForm();
    this.displayEditUserPopup = true;
  }
  confirmEditUser() {
    if (this.selectedUsers.email != this.editUserForm.controls['Email'].value) {
      this.confirmEmail = this.editUserForm.controls['Email'].value;
      this.websiteAPIService
        .checkExistedAndSendChangeConfirmMail(
          this.editUserForm.controls['Email'].value
        )
        .subscribe((res: any) => {
          if (res.data != 'existed') {
            this.displayChangeMailPopup = true;
            this.displayEditUserPopup = false;
            this.confirmCodeSent = res.data;
            this.IsReceiveConfirmCode = true;
            this.messageService.add({
              key: 'bc',
              severity: 'info',
              summary: 'Info',
              detail: 'Confirm change email code was sent!',
            });
          } else {
            this.messageService.add({
              key: 'bc',
              severity: 'error',
              summary: 'Error',
              detail: 'Existed Login Name or Email, try again!',
            });
          }
        });
    } else {
      this.updateUser();
    }
  }
  updateUser() {
    this.editUserForm.controls['ProvinceCode'].setValue(
      this.editUserForm.get('Provinces')?.value.code
    );
    this.editUserForm.controls['DistrictCode'].setValue(
      this.editUserForm.get('Districts')?.value.code
    );
    this.editUserForm.controls['WardCode'].setValue(
      this.editUserForm.get('Wards')?.value.code
    );
    this.editUserForm.controls['Provinces'].setValue(
      this.provinceSelected.name
    );
    this.editUserForm.controls['Districts'].setValue(
      this.districtSelected.name
    );
    this.editUserForm.controls['Wards'].setValue(this.wardSelected.name);
    this.websiteAPIService
      .updateProfile(this.editUserForm.getRawValue())
      .subscribe((res: any) => {
        if (res.data) {
          this.messageService.add({
            key: 'bc',
            severity: 'success',
            summary: 'Successful',
            detail: res.message,
          });
          if (
            this.selectedUsers.loginName !=
            this.editUserForm.controls['LoginName'].value
          ) {
            this.router.navigate(['user/login-user']);
          }
          this.loadDataUser();
          this.displayEditUserPopup = false;
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'error',
            summary: 'Error',
            detail: res.message,
          });
        }
      });
  }
  confirmChangeEmail() {
    if (this.confirmCode == this.confirmCodeSent) {
      this.IsCorrectCode = true;
      this.messageService.add({
        key: 'bc',
        severity: 'info',
        summary: 'Info',
        detail: 'Correct code!',
      });
      this.updateUser();
      this.displayChangeMailPopup = false;
    } else {
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Error',
        detail: 'Wrong confirm code, try again!',
      });
    }
  }
  hideEditUserPopup() {
    this.displayEditUserPopup = false;
    this.submitted = false;
  }
  logout() {
    this.confirmationService.confirm({
      message: 'Are you sure to logout ?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        localStorage.removeItem('authToken');
        this.userData.isLoggedIn = false;
        this.messageService.add({
          key: 'bc',
          severity: 'info',
          summary: 'Info',
          detail: 'Logout successfully!',
        });
        this.loginService.passLogoutAction(true);
        this.router.navigate(['']);
      },
    });
  }
  getAllProvices(): void {
    this.addressService
      .getProvices()
      .pipe()
      .subscribe((data) => {
        this.provices = data;
        if (localStorage.getItem('wards') == null) {
          localStorage.setItem('provinces', data);
        }
      });
  }
  getDistrictsOfProvice(provinceCode: number, districtCode?: any): void {
    this.wards = [];
    this.addressService
      .getDistrictsOfProvince(provinceCode)
      .subscribe((data) => {
        this.districts = data.districts;
        if (localStorage.getItem('districts') == null) {
          localStorage.setItem('districts', data.districts);
        }
      });
  }
  getWardsOfDistrict(districtCode: number, wardCode?: any): void {
    this.addressService.getWardsOfDistrict(districtCode).subscribe((data) => {
      this.wards = data.wards;
      if (localStorage.getItem('wards') == null) {
        localStorage.setItem('wards', data.wards);
      }
    });
  }
  getDistrictsOfProvice2(provinceCode: number): void {
    this.wards = [];
    this.addressService
      .getDistrictsOfProvince(provinceCode)
      .subscribe((data) => {
        this.districts = data.districts;
      });
  }
  getWardsOfDistrict2(districtCode: number): void {
    this.addressService.getWardsOfDistrict(districtCode).subscribe((data) => {
      this.wards = data.wards;
    });
  }
  handleChange() {
    this.editUserForm.get('Provinces')?.valueChanges.subscribe((provices) => {
      this.provinceSelected = provices;
      this.getDistrictsOfProvice2(this.provinceSelected.code);
    });
    this.editUserForm.get('Districts')?.valueChanges.subscribe((districts) => {
      this.districtSelected = districts;
      this.getWardsOfDistrict2(this.districtSelected.code);
    });
    this.editUserForm.get('Wards')?.valueChanges.subscribe((userWard) => {
      this.wardSelected = userWard;
    });
  }
}
