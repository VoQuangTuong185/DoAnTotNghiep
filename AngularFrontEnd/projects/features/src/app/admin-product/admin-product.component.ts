import { Component } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProductDTO } from '../data/ProductDTO.model';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { ActivatedRoute } from '@angular/router';
import { CoreConstants } from '../core/src/lib/core.constant';

@Component({
  selector: 'app-admin-product',
  templateUrl: './admin-product.component.html',
  styleUrls: ['./admin-product.component.scss']
})
export class AdminProductComponent {
  productsDataCols:any[] = [];
  dataProducts!: ProductDTO[];
  existedCategoryId!: number;
  first:number = 10;
  rows:number = 10;
  categoryName?:string;
  constructor(
    private websiteAPIService : WebsiteAPIService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private ActiveRoute : ActivatedRoute,
    ){
    this.productsDataCols = [
      { header : 'STT', field : 'id', width:2, type:'number'},
      { header : 'Hình ảnh', field : 'image', width:5, type:'image'},
      { header : 'Sản phẩm', field : 'productName', width:10, type:'string'},
      { header : 'Giá', field : 'price', width:3, type:'money'},
      { header : 'Giảm giá', field : 'discount', width:3, type:'percent'},
      { header : 'Mô tả', field : 'description', width:3, type:'string'},
      { header : 'Đã bán', field : 'soldQuantity', width:3, type:'string'},
      { header : 'Trạng thái', field : 'isActive', width:3, type:'boolean'},
      { header : 'Thao tác', field : 'action', width:1, type:'button'},
      ];
      this.ActiveRoute.params.subscribe((params) =>{
        this.existedCategoryId = params['categoryId'];
        sessionStorage.setItem('currentCategoryId', params['categoryId']);
      }); 
    }
  ngOnInit() {
    this.loadDataAllProduct();
  }
  loadDataAllProduct(){
    this.websiteAPIService.getProductsByCategoryID(this.existedCategoryId).subscribe((res:any) => {
      this.dataProducts = res.data;  
      this.categoryName = res.data[0].category.categoryName;   
    });
  }
  inActiveProduct(selectedProduct:any){
    let action ='';
    selectedProduct.isActive ? action = 'ẩn': action = 'hiện';
    this.confirmationService.confirm({
      message: 'Xác nhận ' + action + ' sản phẩm ' + selectedProduct.productName + ' ?',
      header: 'Xác nhận',
      icon: 'pi pi-exclamation-triangle', 
      accept: () => {
        this.websiteAPIService.inActiveProduct(selectedProduct.id).subscribe((res:any) => {
          if(res.data){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
            this.loadDataAllProduct();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
          }
        });    
      }
    });
  }
  createImgPath = (serverPath: string) => {
    return CoreConstants.apiUrl() + `/${serverPath}`; 
  }
}
