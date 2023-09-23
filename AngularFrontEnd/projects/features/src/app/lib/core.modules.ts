import { ModuleWithProviders, NgModule } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TabViewModule } from 'primeng/tabview';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { MatInputModule } from '@angular/material/input';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { TooltipModule } from 'primeng/tooltip';
import { FileUploadModule } from 'primeng/fileupload';
import { AccordionModule } from 'primeng/accordion';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { TabMenuModule } from 'primeng/tabmenu';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CalendarModule } from 'primeng/calendar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import {MatDatepickerModule} from '@angular/material/datepicker'; 
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
const NGPrimeModules = [
  HttpClientModule,
  TabViewModule,
  MatSelectModule,
  FormsModule,
  ReactiveFormsModule,
  TableModule,
  CheckboxModule,
  ButtonModule,
  MatInputModule,
  ToolbarModule,
  ConfirmDialogModule,
  ToastModule,
  TooltipModule,
  FileUploadModule,
  AccordionModule,
  InputTextModule,
  InputNumberModule,
  TabMenuModule,
  MatCheckboxModule,
  CommonModule,
  DialogModule,
  MatFormFieldModule,
  MatProgressBarModule,
  CalendarModule,
  MatAutocompleteModule,
  MatIconModule,
  MatDatepickerModule,
  MatProgressSpinnerModule,
  MatChipsModule
];
@NgModule({
  declarations: [],
  imports: [HttpClientModule, NGPrimeModules],
  exports: [],
  providers: [

  ],
})
export class ConfigurationModule {
  static forRoot(): ModuleWithProviders<any> {
    return {
      ngModule: ConfigurationModule,
    };
  }

  constructor() {}
}
