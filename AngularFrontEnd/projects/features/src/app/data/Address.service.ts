import { Injectable } from '@angular/core';
import { Observable, retry, take } from 'rxjs'
import {HttpClient, HttpHeaders} from '@angular/common/http';;

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  ProvincesUri: string = 'https://pprovinces.open-api.vn/api/p/';
  DistrictsUri: string = 'https://pprovinces.open-api.vn/api/d/';

  constructor(private httpClient: HttpClient) { 
  }

  getProvices(): Observable<any> {
    return this.httpClient.get(this.ProvincesUri).pipe(
      retry(3)
    )
  }

  getDistrictsOfProvince(id: number): Observable<any> {
    return this.httpClient.get(`${this.ProvincesUri}${id}?depth=2`).pipe(
      retry(3)
    )
  }

  getWardsOfDistrict(id: number): Observable<any> {
    return this.httpClient.get(`${this.DistrictsUri}${id}?depth=2`).pipe(
      retry(3)
    )
  }
  
}
