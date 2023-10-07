import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService, ConfirmationService } from 'primeng/api';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';

@Component({
  selector: 'app-admin-brand',
  templateUrl: './admin-brand.component.html',
  styleUrls: ['./admin-brand.component.scss']
})
export class AdminBrandComponent {
  brandsDataCols:any[] = [];
  brandForm!: FormGroup;
  dataBrand!: any[];
  first:number = 10;
  rows:number = 10;
  displayEditBrandPopup :boolean = false;
  submitted: boolean = false;
  isEdit: boolean = false;
  existedBrandId!: number;
  constructor(
    private websiteAPIService : WebsiteAPIService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private formBuilder : FormBuilder,
    ){
    this.brandsDataCols = [
      { header : 'STT', field : 'id', width:10, type:'string'},
      { header : 'Brand Name', field : 'brandName', width:25, type:'string'},
      { header : 'Mô tả', field : 'description', width:50, type:'string'},   
      { header : 'Trạng thái', field : 'isActive', width:15, type:'string'},    
      { header : 'Thao tác', field : 'button', width:25, type:'button'},   
      ];
      this.brandForm = this.createEmptyBrandForm();
    }
    createEmptyBrandForm() {
      return this.formBuilder.group({
        Id: [{value: '', disabled: true}],
        BrandName: ['',Validators.required],
        Description: ['']
      })
    }
    ngOnInit() {
      this.loadDataAllBrand();
    }
    loadDataAllBrand(){
      this.websiteAPIService.getAllBrand('admin').subscribe((res:any) => {
        this.dataBrand = res.data; 
      });
    }
    openCreateCategoryPopup(){
      this.brandForm.reset();
      this.displayEditBrandPopup = true;
    }
    openEditBrandPopup(editBrandId: number){
      this.websiteAPIService.getExistedBrand(editBrandId).subscribe((result : any) => {
        if(result.isSuccess){
          var data = result.data;
          this.brandForm = this.createExistedBrandForm(data);
          this.displayEditBrandPopup = true;
          this.isEdit = true; 
        }
        else {
          this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Get edit brand fail!'});
        }
      });  
    }
    createExistedBrandForm(data: any){
      return this.formBuilder.group({
        Id: [{value: data.id, disabled: true}, [Validators.required]],
        BrandName: [data.brandName, [Validators.required]],
        Description: [data.description]
      })
    }
    hideEditUserPopup() {
      this.displayEditBrandPopup = false;
      this.submitted = false;
    }
    confirmForm(isEdit: boolean){
      if (!isEdit){
        this.websiteAPIService.createBrand(this.brandForm.getRawValue()).subscribe((res:any) =>{     
          if(res.isSuccess){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: 'Create brand Successfully!'});
            this.loadDataAllBrand();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Create brand Fail, try again!'});
          }
        });
      }
      else{
        this.websiteAPIService.updateBrand(this.brandForm.getRawValue()).subscribe((res:any) =>{     
          if(res.isSuccess){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: 'Update brand Successfully!'});
            this.loadDataAllBrand();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Update brand Fail, try again!'});
          }
        });
        this.isEdit = false;
      }
      this.hideEditUserPopup();
    }
    inActiveBrand(selectedBrand:any){
      let action ='';
      selectedBrand.isActive ? action = 'ẩn': action = 'hiện';
      this.confirmationService.confirm({
        message: 'Are you sure ' + action + ' brand ' + selectedBrand.brandName + ' ?',
        header: 'Xác nhận',
        icon: 'pi pi-exclamation-triangle', 
        accept: () => {
          this.websiteAPIService.inActiveBrand(selectedBrand.id).subscribe((res:any) => {
            if(res.data){
              this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
              this.loadDataAllBrand();
            }
            else {
              this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
            }
          });    
        }
      });
    }
}
