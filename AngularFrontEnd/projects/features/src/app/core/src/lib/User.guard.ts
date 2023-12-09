import { Inject, Injectable } from '@angular/core';      
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router, UrlTree } from '@angular/router';  
import { Observable } from 'rxjs';
import { UserRole } from '../../../data/UserRole.model';
import { User } from '../../../data/User.model';
import jwt_decode, { JwtPayload } from 'jwt-decode'

@Injectable({      
   providedIn: 'root'      
})      
export class UserGuard implements CanActivate {
   userData = new User();     
   constructor(private router: Router) 
   { 
      this.userData = jwt_decode(localStorage.getItem('authToken')!.replace(/-/g, "+").replace(/_/g, "/"));
   }      
   canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree  {      
      if (this.isUser()) {    debugger
        return true;      
      }           
         this.router.navigate(['admin']);      
      return false;      
}      
public isUser(): boolean {        
   return this.userData.role == UserRole.User;
   }
}  