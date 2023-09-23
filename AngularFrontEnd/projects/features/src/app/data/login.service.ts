import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { User } from './User.model';

@Injectable()
export class LoginService {
  // Observable string sources
  private userLogin = new Subject<User>();
  private userLogout = new Subject<boolean>();
  // Observable string streams
  userLogin$ = this.userLogin.asObservable();
  userLogout$ = this.userLogout.asObservable();
  // Service message commands
  passData(user: User) {
    this.userLogin.next(user);
  }
  passLogoutAction(isLogouted : boolean) {
    this.userLogout.next(isLogouted);
  }
}