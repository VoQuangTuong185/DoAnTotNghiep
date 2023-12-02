import { Injectable } from '@angular/core';      
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router, UrlTree } from '@angular/router';  
import { Observable } from 'rxjs';
import { User } from '../../../data/User.model';
import { AuthService } from './Auth.service';
import { MessageService } from 'primeng/api';
@Injectable({      
   providedIn: 'root'      
})      
export class AuthGuard implements CanActivate {
  userData = new User();     
  constructor(private router: Router, private authService :AuthService, private messageService: MessageService) { }      
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree  {      
    if(localStorage.getItem('authToken') != null && localStorage.getItem('authToken') != 'null' && localStorage.getItem('userRole') != null && localStorage.getItem('userRole') != 'null'){ 
      console.log('if dung')
       this.userData = JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]));
       var date = JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]));
       var dateNow = new Date();
       var tokenExpiredDate = new Date(date.expires)

       let userRole = localStorage.getItem('userRole')!;

        console.log(dateNow)
        console.log(tokenExpiredDate)

       if (dateNow > tokenExpiredDate){
         this.authService.checkValidToken(this.userData.loginName, userRole).subscribe((res: any) => {
          console.log(res) 
           if(res.isSuccess){
            console.log(1)
             localStorage.setItem('authToken', res.data);
             return true;
           }
           else{
            console.log(2)
             localStorage.removeItem('authToken');
             this.messageService.add({key: 'bc', severity:'info', summary: 'Thông tin', detail: res.message});
             this.router.navigate(['login-user']);
             return false;               
           }
         });
       }
       else {
        console.log(3)
        return true;
       }
     } 
     else {
      console.log(4)
      this.router.navigate(['login-user']);
      return false;  
     }
     return true;     
  } 
}