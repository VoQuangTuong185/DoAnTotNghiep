import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnauthorizeComponent } from './authorize/unauthorize/unauthorize.component';
import { AdminGuard } from './core/src/lib/Admin.guard';

const routes: Routes = [
  {
    path:'',
    redirectTo : 'user',
    pathMatch: 'full'
  },
  {
    path:'admin',
    loadChildren: () => import('../../../features/src/app/admin/admin-routing.module').then((m) => m.AdminRoutingModule),
    canActivate : [AdminGuard]
  },
  {
    path:'user',
    loadChildren: () => import('../../../features/src/app/user/user-routing.module').then((m) => m.UserRoutingModule),
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
