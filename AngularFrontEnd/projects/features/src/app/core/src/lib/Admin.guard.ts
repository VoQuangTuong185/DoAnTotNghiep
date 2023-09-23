import { Inject, Injectable } from '@angular/core';      
import { ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router, UrlTree } from '@angular/router';  
import { Observable } from 'rxjs';
import { UserRole } from '../../../data/UserRole.model';
import { User } from '../../../data/User.model';
@Injectable({      
   providedIn: 'root'      
})      
export class AdminGuard implements CanActivate {
   userData = new User();     
   constructor(private router: Router) 
   { 
      this.userData = JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]));
   }      
   canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot):  Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree  {      
      if (this.isAdmin()) {    
        return true;      
      }           
      this.router.navigate(['user/unauthorize']);      
    return false;      
}      
public isAdmin(): boolean {        
   if(this.userData.role == UserRole.Admin){
      return true;
    }
   return false;  
   }
}  