import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
@Injectable({
  providedIn: 'root',
})
export class CheckValidEmailService{
    constructor(private http: HttpClient){}

    async isValidEmail(email: string) : Promise<boolean>{
        const API_KEY = "5a966deda4084afc865ff1062a9639e1";
        const API_URL = "https://emailvalidation.abstractapi.com/v1/?api_key=" + API_KEY + "&email=" + email;
        this.http.get(API_URL).subscribe((res: any) => {
          console.log(res.deliverability)
          return res.deliverability == 'DELIVERABLE';
        }); 
        return false; 
      }
}
