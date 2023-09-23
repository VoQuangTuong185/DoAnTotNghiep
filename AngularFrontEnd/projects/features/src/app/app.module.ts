import {
  CUSTOM_ELEMENTS_SCHEMA,
  DEFAULT_CURRENCY_CODE,
  NgModule,
} from '@angular/core';
import { AccordionModule } from 'primeng/accordion';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from '../../../potal/src/app/app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { HomePageComponent } from './home-page/home-page.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PasswordModule } from 'primeng/password';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogModule } from 'primeng/dialog';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CheckboxModule } from 'primeng/checkbox';
import { MessageModule } from 'primeng/message';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';
import { ImageModule } from 'primeng/image';
import { RegisterComponent } from './register/register.component';
import { AdminUserComponent } from './admin-user/admin-user.component';
import { TableModule } from 'primeng/table';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { RadioButtonModule } from 'primeng/radiobutton';
import { ProgressBarModule } from 'primeng/progressbar';
import { ToastModule } from 'primeng/toast';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TabMenuModule } from 'primeng/tabmenu';
import { CardModule } from 'primeng/card';
import { TabViewModule } from 'primeng/tabview';
import { AvatarModule } from 'primeng/avatar';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TooltipModule } from 'primeng/tooltip';
import { AdminProductComponent } from './admin-product/admin-product.component';
import { AuthGuard } from './core/src/lib/Auth.guard';
import { LoginService } from './data/login.service';
import { CreatProductComponent } from './create-product/create-product.component';
import { UploadImageComponent } from './lib/upload-image/upload-image.component';
import { CarouselModule } from 'primeng/carousel';
import { AuthInterceptor } from './core/src/lib/Auth.interceptor';
import { AuthService } from './core/src/lib/Auth.service';
import { UnauthorizeComponent } from './authorize/unauthorize/unauthorize.component';
import { AdminGuard } from './core/src/lib/Admin.guard';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { InputNumberModule } from 'primeng/inputnumber';
import { AdminCategoryComponent } from './admin-category/admin-category.component';
import { CartComponent } from './cart/cart.component';
import { OrderComponent } from './order/order.component';
import { AdminOrderComponent } from './admin-order/admin-order.component';
import { ListOrderComponent } from './order/listOrder/listOrder.component';
import { OrderDetailComponent } from './order/listOrder/order-detail/order-detail.component';
import { AdminListOrderComponent } from './admin-order/admin-list-order/admin-list-order.component';
import { FooterComponent } from './home-page/footer/footer.component';
import { UploadImageCategoryComponent } from './lib/upload-image-category/upload-image-category.component';
import { AdminBrandComponent } from './admin-brand/admin-brand.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    UnauthorizeComponent,
    HomePageComponent,
    RegisterComponent,
    AdminUserComponent,
    AdminProductComponent,
    CreatProductComponent,
    UploadImageComponent,
    AdminCategoryComponent,
    CartComponent,
    OrderComponent,
    AdminOrderComponent,
    ListOrderComponent,
    OrderDetailComponent,
    AdminListOrderComponent,
    FooterComponent,
    UploadImageCategoryComponent,
    AdminBrandComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AccordionModule,
    PanelModule,
    ButtonModule,
    BrowserAnimationsModule,
    PasswordModule,
    MatDialogModule,
    DialogModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MessageModule,
    InputTextModule,
    RippleModule,
    BrowserModule,
    TableModule,
    CalendarModule,
    DropdownModule,
    ToastModule,
    ProgressBarModule,
    ToastModule,
    NgbModule,
    ConfirmDialogModule,
    TooltipModule,
    CarouselModule,
    InputTextareaModule,
    InputNumberModule,
    RadioButtonModule,
    TabViewModule,
    CardModule,
    TabMenuModule,
    ImageModule,
    ConfirmPopupModule,
    AvatarModule,
    CheckboxModule,
  ],
  providers: [
    {
      provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
      useValue: { appearance: 'outline' },
    },
    MessageService,
    ConfirmationService,
    AuthGuard,
    AdminGuard,
    LoginService,
    AuthService,
    {
      provide: DEFAULT_CURRENCY_CODE,
      useValue: 'USD',
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
