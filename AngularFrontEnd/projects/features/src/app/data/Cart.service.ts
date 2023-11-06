import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { User } from './User.model';

@Injectable()
export class CartService {
  // Observable string sources
  private cartCount = new Subject<boolean>();
  // Observable string streams
  cartCount$ = this.cartCount.asObservable();
  // Service message commands
  passData(value: boolean) {
    this.cartCount.next(value);
  }
}