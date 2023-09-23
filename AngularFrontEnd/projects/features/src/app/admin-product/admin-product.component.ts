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
      { header : 'ID', field : 'id', width:2, type:'string'},
      { header : 'Image', field : 'image', width:5, type:'image'},
      { header : 'Product Name', field : 'productName', width:10, type:'string'},
      { header : 'Price', field : 'price', width:3, type:'money'},
      { header : 'Discount', field : 'discount', width:3, type:'percent'},
      { header : 'Description', field : 'description', width:3, type:'string'},
      { header : 'Sold', field : 'soldQuantity', width:3, type:'string'},
      { header : 'Status', field : 'isActive', width:3, type:'boolean'},
      { header : 'Action', field : 'action', width:1, type:'button'},
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
    selectedProduct.isActive ? action = 'InActive' : action = 'Active';
    this.confirmationService.confirm({
      message: 'Are you sure ' + action + ' course ' + selectedProduct.productName + ' ?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle', 
      accept: () => {
        this.websiteAPIService.inActiveProduct(selectedProduct.id).subscribe((res:any) => {
          if(res.data){
            this.messageService.add({key: 'bc', severity:'success', summary: 'Successful', detail: res.message, life: 3000});
            this.loadDataAllProduct();
          }
          else {
            this.messageService.add({key: 'bc', severity:'error', summary: 'Error', detail: res.message});
          }
        });    
      }
    });
  }
  createImgPath = (serverPath: string) => {
    return CoreConstants.apiUrl() + `/${serverPath}`; 
  }
}
