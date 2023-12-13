import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminProductComponent } from '../admin-product/admin-product.component';
import { AdminUserComponent } from '../admin-user/admin-user.component';
import { AuthGuard } from '../core/src/lib/Auth.guard';
import { CreatProductComponent } from '../create-product/create-product.component';
import { AdminCategoryComponent } from '../admin-category/admin-category.component';
import { AdminOrderComponent } from '../admin-order/admin-order.component';
import { AdminBrandComponent } from '../admin-brand/admin-brand.component';
import { AdminStatisticalComponent } from '../admin-statistical/admin-statistical.component';

const AdminRoutes: Routes = [
  {
    path: '',
    redirectTo: 'admin-category',
    pathMatch: 'full',
  },
  {
    path: 'admin-category',
    children: [
      {
        path: 'create-product',
        component: CreatProductComponent,
        canActivate: [AuthGuard],
      },
      {
        path: 'admin-product/:categoryId',
        children: [
          {
            path: 'create-product',
            component: CreatProductComponent,
            canActivate: [AuthGuard],
          },
          {
            path:'edit-product/:productId',
            component: CreatProductComponent,
            canActivate : [AuthGuard]
          },
          {
            path:'',
            component: AdminProductComponent,
            canActivate: [AuthGuard],
          },
        ],
      },
      {
        path: '',
        component: AdminCategoryComponent,
        canActivate: [AuthGuard],
      },
    ],
  },
  {
    path: 'admin-user',
    component: AdminUserComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin-order',
    component: AdminOrderComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin-brand',
    component: AdminBrandComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'admin-statistical',
    component: AdminStatisticalComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(AdminRoutes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
