import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { LoginUserDTO } from '../data/LoginUserDTO.model';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { AuthService } from '../core/src/lib/Auth.service';
import { LoginService } from '../data/login.service';
import { User } from '../data/User.model';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [MessageService],
})
export class LoginComponent {
  displayForgotPopup: boolean = false;
  displayLoginPopUp: boolean = true;
  displayChangePasswordPopup: boolean = false;
  hide = true;
  loginForm: FormGroup;
  confirmEmail: string = '';
  confirmCode: string = '';
  confirmCodeSent: string = '';
  IsReceiveConfirmCode: boolean = false;
  newPassword: string = '';
  newConfirmPassword: string = '';
  FormChangePassword!: LoginUserDTO;
  previousUrl!: string;

  constructor(
    public dialog: MatDialog,
    private primengConfig: PrimeNGConfig,
    private formBuilder: FormBuilder,
    private websiteAPIService: WebsiteAPIService,
    private router: Router,
    private messageService: MessageService,
    private authService: AuthService,
    private loginService: LoginService
  ) {
    this.loginForm = this.createEmptyLoginForm();
  }
  createEmptyLoginForm() {
    return this.formBuilder.group({
      LoginUser: ['', Validators.required],
      Password: ['', Validators.required],
    });
  }
  ngOnInit() {
    this.primengConfig.ripple = true;
    this.FormChangePassword = new LoginUserDTO();
  }
  openLoginPopUp() {
    this.displayLoginPopUp = true;
  }
  login() {
    this.authService
      .login(this.loginForm.getRawValue())
      .subscribe((res: any) => {
        if (res.isSuccess) {
          localStorage.setItem('authToken', res.data);
          const userDetails = new User();
          var result = JSON.parse(
            window.atob(localStorage.getItem('authToken')!.split('.')[1])
          );
          userDetails.id = result.id;
          userDetails.loginName = result.loginName;
          userDetails.isLoggedIn = true;
          userDetails.role = result.role;
          this.loginService.passData(userDetails);
          this.displayLoginPopUp = false;
          this.messageService.add({
            key: 'bc',
            severity: 'success',
            summary: '',
            detail: 'Login successfully, navigating to home...',
          });
          setTimeout(()=>{ this.router.navigate(['user/home-page']); }, 2000)          
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'error',
            summary: 'Error',
            detail: res.data,
          });
        }
      });
  }
  openForgotPopup() {
    this.displayForgotPopup = true;
    this.displayLoginPopUp = false;
  }
  sendForgetCode() {
    this.websiteAPIService
      .sendForgetCode(this.confirmEmail)
      .subscribe((res: any) => {
        if (res.data != '') {
          this.messageService.add({
            key: 'bc',
            severity: 'info',
            summary: 'Info',
            detail: 'Confirm forget password code was sent!',
          });
          this.IsReceiveConfirmCode = true;
          this.confirmCodeSent = res.data;
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'error',
            summary: 'Error',
            detail: 'Not existed this email, try again!',
          });
        }
      });
  }
  confirmForgetPassword() {
    if (this.confirmCode == this.confirmCodeSent) {
      this.displayChangePasswordPopup = true;
      this.messageService.add({
        key: 'bc',
        severity: 'info',
        summary: 'Info',
        detail: 'Correct code, please enter new password!',
      });
    } else {
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Error',
        detail: 'Wrong confirm code, try again!',
      });
    }
  }
  changePassword() {
    if (this.newPassword != this.newConfirmPassword) {
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Error',
        detail: 'Passwords you entered does not match, try again!',
      });
      return;
    }
    this.FormChangePassword.loginUser = this.confirmEmail;
    this.FormChangePassword.password = this.newPassword;
    this.websiteAPIService
      .changePassword(this.FormChangePassword)
      .subscribe((res: any) => {
        if (res.data) {
          this.messageService.add({
            key: 'tc',
            severity: 'info',
            summary: 'success',
            detail: 'Change password successfully!',
          });
          this.displayLoginPopUp = false;
          this.messageService.add({
            key: 'c',
            sticky: true,
            severity: 'success',
            summary: 'Are you sure?',
            detail: 'Go to login page now',
          });
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'error',
            summary: 'Error',
            detail: 'Change password fail, try again!',
          });
        }
      });
  }
  onConfirmLoginPage() {
    this.messageService.clear();
    this.displayLoginPopUp = true;
    this.displayForgotPopup = false;
    this.displayChangePasswordPopup = false;
  }
  onRejectLoginPage() {
    this.messageService.clear();
  }
  onConfirmCoursePage() {
    this.messageService.clear();
    this.displayLoginPopUp = false;
    this.router.navigate(['user/home-page']);
  }
  onRejectLoginCoursePage() {
    this.messageService.clear();
  }
}
