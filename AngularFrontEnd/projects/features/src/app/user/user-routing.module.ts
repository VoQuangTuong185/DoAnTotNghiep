import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/src/lib/Auth.guard';
import { CartComponent } from '../cart/cart.component';
import { OrderComponent } from '../order/order.component';
const UserRoutes: Routes = [
  {
    path: '',
    redirectTo: 'home-page',
    pathMatch: 'full',
  },
  {
    path: 'cart',
    component: CartComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'order',
    component: OrderComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(UserRoutes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
