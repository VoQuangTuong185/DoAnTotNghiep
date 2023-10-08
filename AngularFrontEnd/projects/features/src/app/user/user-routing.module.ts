import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnauthorizeComponent } from '../authorize/unauthorize/unauthorize.component';
import { LoginComponent } from '../login/login.component';
import { HomePageComponent } from '../home-page/home-page.component';
import { RegisterComponent } from '../register/register.component';
import { AuthGuard } from '../core/src/lib/Auth.guard';
import { CartComponent } from '../cart/cart.component';
import { OrderComponent } from '../order/order.component';
import { ProductDetailComponent } from '../product-detail/product-detail.component';
const UserRoutes: Routes = [
  {
    path: '',
    redirectTo: 'home-page',
    pathMatch: 'full',
  },
  {
    path: 'login-user',
    component: LoginComponent,
  },
  {
    path: 'home-page',
    component: HomePageComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'register-user',
    component: RegisterComponent,
  },
  {
    path: 'unauthorize',
    component: UnauthorizeComponent,
  },
  {
    path: 'cart',
    component: CartComponent,
  },
  {
    path: 'order',
    component: OrderComponent,
  },
  {
    path: 'product-detail/:id',
    component: ProductDetailComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(UserRoutes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
