import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { CoreConstants } from '../core/src/lib/core.constant';
import { FeedbackDetailShow } from '../data/FeedbackDetailShow.model';

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
  constructor(
    private router: Router,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private websiteAPIService: WebsiteAPIService,
    private ActiveRoute : ActivatedRoute,
  ){
    this.ActiveRoute.params.subscribe((params) =>{
      this.existedProductId = params['id'];
      this.getExistedProduct();
      this.getFeedbacks();
    });
  }
  ngOnInit(): void{
    this.responsiveOptions = [{ breakpoint: '1024px',  numVisible: 5 }, { breakpoint: '768px', numVisible: 3 }, { breakpoint: '560px', numVisible: 1 }];
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
  openAdminReplyPopup(){
    this.displayAdminReplyPopup = true;
  }
  hideAdminReplyPopup(){
    this.displayAdminReplyPopup = false;
  }
}
