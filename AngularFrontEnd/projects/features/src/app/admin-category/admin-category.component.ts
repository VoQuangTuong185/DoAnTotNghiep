import { Component } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProductDTO } from '../data/ProductDTO.model';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CoreConstants } from '../core/src/lib/core.constant';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-category',
  templateUrl: './admin-category.component.html',
  styleUrls: ['./admin-category.component.scss']
})
export class AdminCategoryComponent {
  productsDataCols:any[] = [];
  dataCategories!: ProductDTO[];
  first:number = 10;
  rows:number = 10;
  categoryForm!: FormGroup;
  existedCategoryId!: number;
  displayEditUserPopup :boolean = false;
  response!: {dbPath: ''};
  submitted: boolean = false;
  isEdit: boolean = false;
  constructor(
    private websiteAPIService : WebsiteAPIService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private formBuilder : FormBuilder,
    ){
    this.productsDataCols = [
      { header : 'STT', field : 'id', width:7, type:'string'},
      { header : 'Hình ảnh', field : 'image', width:15, type:'image'},
      { header : 'Danh mục', field : 'categoryName', width:23, type:'string'},
      { header : 'Mô tả', field : 'description', width:35, type:'string'},   
      { header : 'Trạng thái', field : 'isActive', width:10, type:'string'},    
      { header : 'Thao tác', field : 'button', width:20, type:'button'},   
      ];
      this.categoryForm = this.createEmptyUserForm();
    }
    createEmptyUserForm() {
      return this.formBuilder.group({
        Id: [{value: '', disabled: true}],
        CategoryName: ['',Validators.required],
        Image: ['',Validators.required],
        Description: ['']
      })
    }
  ngOnInit() {
    this.loadDataAllCategory();
  }
  openCreateCategoryPopup(){
    this.categoryForm.reset();
    this.displayEditUserPopup = true;
  }
  openEditCategoryPopup(editCategoryId: number){
    this.websiteAPIService.getExistedCategory(editCategoryId).subscribe((result : any) => {
      if(result.isSuccess){
        var data = result.data;
        this.categoryForm = this.createExistedUserForm(data);
        this.displayEditUserPopup = true;
        this.isEdit = true; 
      }
      else {
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Không thể lấy thông tin danh mục!'});
      }
    });  
  }
  loadDataAllCategory(){
    this.websiteAPIService.getAllCategory('admin').subscribe((res:any) => {
      this.dataCategories = res.data; 
    });
  }
  inActiveCategory(selectedCategory:any){
    let action ='';
    selectedCategory.isActive ? action = 'ẩn': action = 'hiện';
    this.confirmationService.confirm({
      message: 'Xác nhận ' + action + ' danh mục ' + selectedCategory.categoryName + ' ?',
      header: 'Xác nhận',
      icon: 'pi pi-exclamation-triangle', 
      accept: () => {
        this.websiteAPIService.inActiveCategory(selectedCategory.id).subscribe((res:any) => {
          if(res.data){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
            this.loadDataAllCategory();
          }
          else {
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
          }
        });    
      }
    });
  }
  createExistedUserForm(data: any){
    return this.formBuilder.group({
      Id: [{value: data.id, disabled: true}, [Validators.required]],
      CategoryName: [data.categoryName, [Validators.required]],
      Image: [data.image, [Validators.required]],
      Description: [data.description]
    })
  }
  createImgPath = (serverPath: string) => {
    return CoreConstants.apiCategoryURL() + `/${serverPath}`; 
  }
  uploadFinished = (event:any) => { 
    this.response = event; 
    this.categoryForm.controls['Image'].setValue(event.dbPath);
  }
  confirmForm(isEdit: boolean){
    if(this.categoryForm.invalid){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập các thông tin bắt buộc!',
      });
      return;
    }
    if (!isEdit){
      this.websiteAPIService.createCategory(this.categoryForm.getRawValue()).subscribe((res:any) =>{     
        if(res.isSuccess){
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message});
          this.loadDataAllCategory();
        }
        else {
          this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
        }
      });
    }
    else{
      this.websiteAPIService.updateCategory(this.categoryForm.getRawValue()).subscribe((res:any) =>{     
        if(res.isSuccess){
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message});
          this.loadDataAllCategory();
        }
        else {
          this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
        }
      });
    }
    this.hideEditUserPopup();
  }
  hideEditUserPopup() {
    this.displayEditUserPopup = false;
    this.submitted = false;
  }
}
