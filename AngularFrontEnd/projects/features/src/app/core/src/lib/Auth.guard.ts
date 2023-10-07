import { Injectable } from '@angular/core';      
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router, UrlTree } from '@angular/router';  
import { Observable } from 'rxjs';
import { UserRole } from '../../../data/UserRole.model';
import { User } from '../../../data/User.model';
import { AuthService } from './Auth.service';
import { MessageService } from 'primeng/api';
import { getCookie, getCookies, setCookie } from 'typescript-cookie'
@Injectable({      
   providedIn: 'root'      
})      
export class AuthGuard implements CanActivate {
   userData = new User();     
   constructor(private router: Router, private authService :AuthService,private messageService: MessageService) { }      
   canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree  {      
    var isContinue = false;
    if(localStorage.getItem('authToken') != null && localStorage.getItem('authToken') != 'null'){ 
       this.userData = JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]));
       var date = JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]));
       var dateNow = new Date();
       var tokenExpiredDate = new Date(date.expires)
       if (dateNow > tokenExpiredDate){
         this.authService.checkValidToken(this.userData.loginName).subscribe((res: any) => { 
           if(res.isSuccess){
             localStorage.setItem('authToken', res.data);
             return true;
           }
           else{
             localStorage.removeItem('authToken');
             this.messageService.add({key: 'bc', severity:'info', summary: 'Thông tin', detail: res.message});
             this.router.navigate(['/user/login-user']);
             return false;               
           }
         });
       }
       else {
        return true;
       }
     }  
     return true;
    }     
}