import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { LoginUserDTO } from '../data/LoginUserDTO.model';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { AuthService } from '../core/src/lib/Auth.service';
import { LoginService } from '../data/Login.service';
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
  checked: boolean = false;
  userRole: 'admin' | 'user' = 'user';

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
    if(this.loginForm.invalid){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập các thông tin bắt buộc để đăng nhập!',
      });
      return;
    }
    localStorage.setItem('userRole', this.userRole);
    this.authService
      .login(this.loginForm.getRawValue(), this.userRole)
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
            detail: res.message,
          });
          let navigateURL = this.userRole == 'user' ? 'home-page' : 'admin/admin-category';
          setTimeout(()=>{ this.router.navigate([navigateURL]); })          
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'error',
            summary: 'Lỗi',
            detail: res.message,
          });
        }
      });
  }
  openForgotPopup() {
    this.displayLoginPopUp = false;
    this.displayForgotPopup = true;   
  }
  sendForgetCode() {
    if(this.confirmEmail.trimStart() == ''){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập các thông tin bắt buộc để quên mật khẩu!',
      });
      return;
    }
    this.websiteAPIService
      .sendForgetCode(this.confirmEmail)
      .subscribe((res: any) => {
        if (res.data != '') {
          this.messageService.add({
            key: 'bc',
            severity: 'info',
            summary: 'Thông tin',
            detail: 'Mã xác nhận quên mật khẩu đã được gửi đi!',
          });
          this.IsReceiveConfirmCode = true;
          this.confirmCodeSent = res.data;
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'error',
            summary: 'Lỗi',
            detail: 'Không tìm thấy email này, hãy thử lại!',
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
        summary: 'Thông tin',
        detail: 'Xác nhận thành công, hãy nhập mật khẩu mới!',
      });
    } else {
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Xác nhận không thành công, hãy thử lại!',
      });
    }
  }
  changePassword() {
    if (this.newPassword != this.newConfirmPassword) {
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Mật khẩu xác nhận không khớp, hãy thử lại!',
      });
      return;
    }
    this.FormChangePassword.email = this.confirmEmail;
    this.FormChangePassword.password = this.newPassword;
    this.websiteAPIService
      .changePassword(this.FormChangePassword)
      .subscribe((res: any) => {
        if (res.data) {
          this.messageService.add({
            key: 'tc',
            severity: 'info',
            summary: 'success',
            detail: 'Đổi mật khẩu thành công!',
          });
          this.displayLoginPopUp = false;
          this.messageService.add({
            key: 'c',
            sticky: true,
            severity: 'success',
            summary: 'Xác nhận?',
            detail: 'Quay về trang đăng nhập',
          });
        } else {
          this.messageService.add({
            key: 'bc',
            severity: 'error',
            summary: 'Lỗi',
            detail: 'Đổi mật khẩu thất bại, hãy thử lại!',
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
    this.router.navigate(['home-page']);
  }
  onRejectLoginCoursePage() {
    this.messageService.clear();
  }
  hideLogin(){
    if(!this.displayForgotPopup){
      this.router.navigate(['home-page']);
    }
  }
  handleUserRole(e:any){
    this.userRole = e.value;
  }
}
