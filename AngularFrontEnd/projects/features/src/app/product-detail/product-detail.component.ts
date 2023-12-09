import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { CoreConstants } from '../core/src/lib/core.constant';
import { FeedbackDetailShow } from '../data/FeedbackDetailShow.model';
import { User } from '../data/User.model';
import { AddCart } from '../data/AddCart.model';
import { CartService } from '../data/Cart.service';
import jwt_decode, { JwtPayload } from 'jwt-decode'

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  existedProductId!: number;
  images: any[] = [];
  responsiveOptions: any[] = [];
  @Input() value: unknown
  currentProduct!: any;
  @Output() valueChange = new EventEmitter<unknown>();
  quantityAddToCart: number = 1;
  userRole!: string;
  feedbacks!: FeedbackDetailShow[];
  displayAdminReplyPopup: boolean = false;
  adminReplyContent!: string;
  currentFeedback!: FeedbackDetailShow;
  userData = new User();
  quantityCurrentProductInCard: number = 0;
  products: any[] = [];
  addCart = new AddCart();
  constructor(
    private router: Router,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private websiteAPIService: WebsiteAPIService,
    private ActiveRoute : ActivatedRoute,
    private cartService: CartService,
  ){
    this.ActiveRoute.params.subscribe((params) =>{
      this.existedProductId = params['id'];
      this.getExistedProduct();
      this.getFeedbacks();
    });
    if (localStorage.getItem('authToken') != null && localStorage.getItem('authToken') != 'null') {
      this.userData = jwt_decode(localStorage.getItem('authToken')!.replace(/-/g, "+").replace(/_/g, "/"));
      this.userData.isLoggedIn = true;
    }
  }
  ngOnInit(): void{
    this.responsiveOptions = [{ breakpoint: '1024px',  numVisible: 5 }, { breakpoint: '768px', numVisible: 3 }, { breakpoint: '560px', numVisible: 1 }];
    this.getCartByUserID();
  }
  getExistedProduct(){
    this.userRole = localStorage.getItem('userRole')!;
    if (this.userRole == 'admin'){
      this.websiteAPIService.getExistedProductAdmin(this.existedProductId).subscribe((result : any) => {
        this.currentProduct = result.data;
        let currentImageDetail = this.currentProduct.imageDetail || [];
        currentImageDetail.unshift(this.currentProduct.image);
        this.currentProduct.imageDetail = currentImageDetail;
      });
    }
    else {
      this.websiteAPIService.getExistedProductUser(this.existedProductId).subscribe((result : any) => {
        this.currentProduct = result.data;
        let currentImageDetail = this.currentProduct.imageDetail || [];
        currentImageDetail.unshift(this.currentProduct.image);
        this.currentProduct.imageDetail = currentImageDetail;
      });
    }
  }
  createImgPath = (serverPath: string) => {
    this.userRole = localStorage.getItem('userRole')!;
    let apiUrl = this.userRole == 'admin' ? CoreConstants.apiAdminURL() : CoreConstants.apiUrl();
    return apiUrl + `/${serverPath}`; 
  }
  getFeedbacks(){
    if (this.userRole == 'admin'){
      this.websiteAPIService.getFeedbackByProductIdAdmin(this.existedProductId).subscribe((result : any) => {
        this.feedbacks = result.data;
      });
    }
    else {
      this.websiteAPIService.getFeedbackByProductIdUser(this.existedProductId).subscribe((result : any) => {
        this.feedbacks = result.data;
      });
    }
  }
  openAdminReplyPopup(feedback: FeedbackDetailShow){
    this.currentFeedback = feedback;
    this.displayAdminReplyPopup = true;
    this.adminReplyContent = this.currentFeedback.adminReply || '';
  }
  hideAdminReplyPopup(){
    this.displayAdminReplyPopup = false;
  }
  sumbitAdminReply(){
    if(this.adminReplyContent == '' || this.adminReplyContent == undefined || (this.adminReplyContent != undefined && this.adminReplyContent.trimStart() == '')){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập nội dung phản hồi!',
      });
      return;
    }
    this.currentFeedback.adminReply = this.adminReplyContent;
    this.websiteAPIService.replyFeedback(this.currentFeedback).subscribe((res: any) => {
      if (res.data) 
        this.messageService.add({ key: 'bc', severity: 'success', summary: 'Thành công', detail: res.message});
      else 
        this.messageService.add({ key: 'bc', severity: 'error', summary: 'Lỗi', detail: res.message });
      
      this.displayAdminReplyPopup = false;
      this.getExistedProduct();
      this.getFeedbacks();
    });
  }
  getCartByUserID(){
    this.websiteAPIService.getCartByUserID(Number(this.userData.id)).subscribe((res: any) => {
      this.products = res.data;
      let currentProduct = this.products.filter(x => x.productId == this.currentProduct.id);
      if (currentProduct.length > 0 &&  currentProduct[0].quanity > 0){
        this.quantityCurrentProductInCard = currentProduct[0].quanity;
      }    
    });   
  }
  addToCart(isBuyNow:boolean) {
    if (this.currentProduct.quanity < (this.quantityCurrentProductInCard + this.quantityAddToCart)){
      this.messageService.add({ key:'bc', severity: 'error', summary: 'Lỗi', detail: 'Số lượng sản phẩm có sẵn không đủ (bao gồm số lượng đã có sẵn trong giỏ hàng)'});
      return;
    }
    this.addCart.ProductId = this.currentProduct.id;
    this.addCart.UserId = Number(this.userData.id);
    this.addCart.Quanity = this.quantityAddToCart;
    this.quantityAddToCart = 1;
    this.websiteAPIService.addCart(this.addCart).subscribe((res: any) => {
      if (res.data) {
        this.messageService.add({
          key: 'bc',
          severity: 'success',
          summary: 'Thành công',
          detail: res.message,
          life: 3000,
        });
        this.cartService.passData(true);
        this.getCartByUserID();
        if (isBuyNow)
          this.router.navigate(['/user/cart']);
      } else {
        this.messageService.add({
          key: 'bc',
          severity: 'error',
          summary: 'Lỗi',
          detail: res.message,
        });
      }
    });
  }
}
