import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { RegisterUserDTO } from '../../../data/RegisterUserDTO.model';
import { LoginUserDTO } from '../../../data/LoginUserDTO.model';
import { RegisterConstant } from '../../../RegisterCourse.constant';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private urlRegister = 'register';
  private urlLogin = 'login';
  private urlCheckValidToken = 'check-valid-token';
  private headers = { 'content-type': 'application/json'}  
  constructor(private http: HttpClient) {}
  public register(user: RegisterUserDTO): Observable<any> {
    return this.http.post<any>(RegisterConstant.libraryApiUrlAuth() + this.urlRegister,user);
  }
  public login(user: LoginUserDTO): Observable<string> {
    return this.http.post<any>(RegisterConstant.libraryApiUrlAuth()+ this.urlLogin, user,{withCredentials: true});
  }
  public checkValidToken(loginName : string): Observable<any> {
    return this.http.post<any>(RegisterConstant.libraryApiUrlAuth()+ this.urlCheckValidToken ,JSON.stringify(loginName),{'headers':this.headers, withCredentials: true});
  }
}