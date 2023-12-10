import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { CoreConstants } from '../core/src/lib/core.constant';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { filter } from 'rxjs';
@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.scss']
})
export class CreatProductComponent {
  productForm: FormGroup;
  isEditing:boolean = false;
  existedCategoryId!: number;
  existedProductId!: number;
  ProductID!: number;
  response!: {dbPath: ''};
  brandData!: any[];
  brands: any[];
  brandSelected: any;
  constructor(
    private formBuilder : FormBuilder, 
    private confirmationService: ConfirmationService, 
    private router: Router, 
    private ActiveRoute : ActivatedRoute,
    private websiteAPIService : WebsiteAPIService,
    private messageService: MessageService,
    private http: HttpClient)
    {
      this.brands = [];
      this.ActiveRoute.paramMap.subscribe(()=> {
        this.existedCategoryId = Number(window.history.state.existedCategoryId);
      });
      this.productForm = this.createEmptyCourseForm();
      this.ActiveRoute.params.subscribe((params) =>{
        this.existedProductId = params['productId'];
      });
  }
  ngOnInit(): void{
    this.existedProductId ? this.getExistedProduct() : this.autoGeneratedProductID();  
    this.loadDataAllBrands(); 
    this.productForm.get('BrandName')?.valueChanges.subscribe(brand => {
      this.brandSelected = brand;
      this.getBrandIdOfBrand();
    });
  }
  createEmptyCourseForm(){
    return this.formBuilder.group({
      Id: [{value: '', disabled: true},Validators.required],
      ProductName: ['',[Validators.required, Validators.maxLength(255)]],
      Description: ['', Validators.maxLength(255)],
      BrandName: ['',Validators.required],
      BrandId: [{value: null, disabled: true},Validators.required],
      CategoryId: [{value: this.existedCategoryId, disabled:true}, Validators.required],
      SoldQuantity: [{value: null, disabled: true}],
      Quanity: [null, [Validators.required, Validators.max(1000), Validators.min(0)]],
      Discount: [null, [Validators.required, Validators.max(99), Validators.min(0)]],
      Price: [null, Validators.min(1000)],
      Image: ['', Validators.required],
      IsActive: [true],
      ImageDetail: [''],
    })
  }
  createExistedCourseForm(data : any){
    return this.formBuilder.group({
      Id: [{value: data.id, disabled:true},[Validators.required]],
      ProductName: [data.productName,[Validators.required, Validators.maxLength(255)]],
      Description: [data.description, [Validators.maxLength(255)]],
      BrandName: [data.brand.brandName,[Validators.required]],
      BrandId: [{value: data.brandId, disabled: true}, [Validators.required]],
      CategoryId: [{value: data.categoryId, disabled: true},[Validators.required]],
      SoldQuantity: [{value: data.soldQuantity, disabled:true},[Validators.required]],
      Quanity: [data.quanity, [Validators.required, Validators.max(100), Validators.min(0)]],
      Discount: [data.discount,[Validators.required, Validators.max(99), Validators.min(0)]],
      Price: [data.price, [Validators.required, Validators.min(1000)]],
      Image: [data.image, [Validators.required]],
      ImageDetail: [data.imageDetail],
    })
  }
  getBrandIdOfBrand(): void {
    let tempData = this.brandData.find(({ brandName }) => brandName == this.brandSelected);
    this.productForm.controls['BrandId'].setValue(tempData.id);
  }
  saveForm(){
    if(this.productForm.controls['Image'].value == ''){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy tải lên hình ảnh mô tả!',
      });
      return;
    }
    if(this.productForm.invalid){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập các thông tin bắt buộc!',
      });
      return;
    }
    var categoryId = sessionStorage.getItem('currentCategoryId')
    if(this.existedProductId) {  
      this.websiteAPIService.updateProduct(this.productForm.getRawValue()).subscribe((res:any) =>{
        if(res.isSuccess){
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message});
          this.router.navigate(['/admin/admin-category/admin-product/'+ categoryId]);
        }
        else {
          this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
        }
      });
    }
    else{   
      this.websiteAPIService.createProduct(this.productForm.getRawValue()).subscribe((res:any) =>{
        if(res.isSuccess){
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message});
          this.router.navigate(['/admin/admin-category/admin-product/'+ categoryId]);
        }
        else {
          this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
        }
      });
    }
  }
  cancel(){
    this.confirmationService.confirm({
      message: 'Huỷ tạo/chỉnh sửa sản phẩm ?',
      header: 'Xác nhận',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {        
        this.backToParent(); 
      }
    }); 
  }
  backToParent(){
    var categoryId = sessionStorage.getItem('currentCategoryId')
    this.router.navigate(['/admin/admin-category/admin-product/'+ categoryId]);
  }
  getExistedProduct(){
    this.websiteAPIService.getExistedProductAdmin(this.existedProductId).subscribe((result : any) => {
      var data = result.data;
      this.productForm = this.createExistedCourseForm(data);
    });
  }
  autoGeneratedProductID(){
    this.websiteAPIService.autoGeneratedProductID().subscribe((res:any) =>{
      if(res.isSuccess){
        this.ProductID = res.data;
        this.productForm.controls['Id'].setValue(this.ProductID);
      }
      else {
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Chức năng tạo mã sản phẩm bị lỗi!'});
      }
    });
  }  
  createImgPath = (serverPath: string) => {
    return CoreConstants.apiAdminURL() + `/${serverPath}`; 
  }
  uploadImageFinished = (event:any) => { 
    this.response = event; 
    this.productForm.controls['Image'].setValue(event.dbPath);
  }
  uploadImageDetailFinished = (event:any) => { 
    this.response = event;  
    let currentImageDetail = this.productForm.controls['ImageDetail'].value || [];
    currentImageDetail.push(event.dbPath)
    this.productForm.controls['ImageDetail'].setValue(currentImageDetail);
  }
  loadDataAllBrands(){
    this.websiteAPIService.getAllBrand('active').subscribe((res:any) => {
      this.brandData = res.data; 
      this.brandData.forEach(element => {
        this.brands.push(element.brandName);
      });
    });
  }
  changeBrandName(){
    this.productForm.get('BrandName')?.valueChanges.subscribe(brand => {
      this.brandSelected = brand;
      this.getBrandIdOfBrand();
    });
  }
  removeImage(imagePath: string){
    var currentImageDetail = this.productForm.controls['ImageDetail'].value;
    currentImageDetail = currentImageDetail.filter((obj: string) => {return obj !== imagePath});
    this.productForm.controls['ImageDetail'].setValue(currentImageDetail);
  }
}
