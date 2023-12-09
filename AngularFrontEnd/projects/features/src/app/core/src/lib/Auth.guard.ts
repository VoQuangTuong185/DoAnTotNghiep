import { Injectable } from '@angular/core';      
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router, UrlTree } from '@angular/router';  
import { Observable } from 'rxjs';
import { User } from '../../../data/User.model';
import { AuthService } from './Auth.service';
import { MessageService } from 'primeng/api';
import jwt_decode, { JwtPayload } from 'jwt-decode'

@Injectable({      
   providedIn: 'root'      
})      
export class AuthGuard implements CanActivate {
  userData = new User();     
  constructor(private router: Router, private authService :AuthService, private messageService: MessageService) { }      
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree  {      
    if(localStorage.getItem('authToken') != null && localStorage.getItem('authToken') != 'null' && localStorage.getItem('userRole') != null && localStorage.getItem('userRole') != 'null'){ 
       this.userData = jwt_decode(localStorage.getItem('authToken')!.replace(/-/g, "+").replace(/_/g, "/"));
       var dateNow = new Date(); 
       var tokenExpiredDate = new Date(this.userData.expires);
       let userRole = localStorage.getItem('userRole')!; 
       if (dateNow > tokenExpiredDate){
         this.authService.checkValidToken(this.userData.loginName, userRole).subscribe((res: any) => {
           if(res.isSuccess){
             localStorage.setItem('authToken', res.data);
             return true;
           }
           else{
             localStorage.removeItem('authToken');
             this.messageService.add({key: 'bc', severity:'info', summary: 'Thông tin', detail: res.message});
             this.router.navigate(['login-user']);
             return false;               
           }
         });
       }
       else {
        return true;
       }
     } 
     else {
      console.log('11')
      this.router.navigate(['login-user']);
      return false;  
     }
     console.log('1')
     return true;     
  } 
}