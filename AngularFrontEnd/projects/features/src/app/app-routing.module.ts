import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnauthorizeComponent } from './authorize/unauthorize/unauthorize.component';
import { AdminGuard } from './core/src/lib/Admin.guard';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { UserGuard } from './core/src/lib/User.guard';
import { HomePageComponent } from './home-page/home-page.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './core/src/lib/Auth.guard';

const routes: Routes = [
  {
    path:'',
    redirectTo : 'home-page',
    pathMatch: 'full'
  },
  {
    path: 'home-page',
    component: HomePageComponent,
  },
  {
    path: 'login-user',
    component: LoginComponent,
  },
  {
    path: 'register-user',
    component: RegisterComponent,
  },
  {
    path: 'product-detail/:id',
    component: ProductDetailComponent,
  },
  {
    path: 'unauthorize',
    component: UnauthorizeComponent,
  },
  {
    path:'admin',
    loadChildren: () => import('../../../features/src/app/admin/admin-routing.module').then((m) => m.AdminRoutingModule),
    canActivate : [AdminGuard]
  },
  {
    path:'user',
    loadChildren: () => import('../../../features/src/app/user/user-routing.module').then((m) => m.UserRoutingModule),
    canActivate : [AuthGuard]
  },
  {
    path:'unauthorize',
    component: UnauthorizeComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
