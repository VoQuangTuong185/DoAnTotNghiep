import { Component } from '@angular/core';
import { ConfirmationService, MenuItem, MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { ProductDTO } from '../data/ProductDTO.model';
import { LoginService } from '../data/Login.service';
import { User } from '../data/User.model';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { CoreConstants } from '../core/src/lib/core.constant';
import { AddressService } from '../data/Address.service';
import { AddCart } from '../data/AddCart.model';
import { Router } from '@angular/router';
import { CartService } from '../data/Cart.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss'],
})
export class HomePageComponent {
  responsiveOptions;
  currentProduct!: any;
  displayDetailPopup: boolean = false;
  loginedDataProduct!: any[];
  unloginedDataProduct!: ProductDTO[];
  userId: number = 0;
  IsLogin: boolean = false;
  displayFeedbackPopup: boolean = false;
  feedbackContent: string = '';
  subscription: Subscription;
  userData = new User();
  quantityAddToCart: number = 1;
  provices: any[];
  lstProduct: ProductDTO[] = []
  lstCate: MenuItem[] = [];
  activeCate: any;
  addCart = new AddCart();
  products: any[] = [];
  quantityCurrentProductInCard: number = 0;
  cloneDatProduct !: any[];
  visible: boolean = true;
  userRole!: string;
  numberCart: number = 0;
  constructor(
    private websiteAPIService: WebsiteAPIService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private loginService: LoginService,
    private addressService: AddressService,
    private router: Router,
    private cartService: CartService,
  ) {
    this.responsiveOptions = [
      { breakpoint: '1024px', numVisible: 4, numScroll: 4 },
      { breakpoint: '768px', numVisible: 3, numScroll: 3 },
      { breakpoint: '560px', numVisible: 2, numScroll: 2 },
      { breakpoint: '460px', numVisible: 1, numScroll: 1 },
    ];
    this.subscription = loginService.userLogout$.subscribe((isLogouted) => {
      if (isLogouted) {
        this.userData.isLoggedIn = false;
        this.getAllProduct();
      }
    });
    this.userRole = localStorage.getItem('userRole')!;
    if (localStorage.getItem('authToken') != null && localStorage.getItem('authToken') != 'null') {
      this.userData = JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]));
      this.userData.isLoggedIn = true;
    }
    this.provices = [];
  }
  ngOnInit() {
    this.getAllProduct();
    this.getAllCategory();
    this.activeCate = this.lstCate[0];
  }
  getAllProduct() {
    this.websiteAPIService.getAllProduct().subscribe((res: any) => {
      this.unloginedDataProduct = res.data;
      this.loginedDataProduct = res.data;
      this.lstProduct = res.data
    });
  }
  getAllCategory() {
    this.websiteAPIService.getAllCategoryUser().subscribe((res: any) => {
      if (res.isSuccess === true) {
        this.lstCate = [
          {
            id: -1,
            categoryName: 'Tất cả',
            image:
              'https://storage.googleapis.com/ops-shopee-files-live/live/shopee-blog/2020/07/do-dung-hoc-tap-can-thiet-cho-lop-6-thumb-ngang.png',
          },
          ...res.data,
        ];
        return;
      }
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: res.message,
      });
    });
  }
  handleChangeCate(cateId: number) {
    if (cateId === -1) {
      this.websiteAPIService.getAllProduct().subscribe((res: any) => {
        this.lstProduct = res.data
      });
      return;
    }
    this.cloneDatProduct = Object.values({...this.loginedDataProduct});
    this.lstProduct = this.cloneDatProduct.filter((x) => x.category.id === cateId);  
    this.updateVisibility()
  }
  getProductByCategory(categoryId: number) {
    this.websiteAPIService
      .getProductsByCategoryIDUser(categoryId)
      .subscribe((res: any) => {
        if (res.isSuccess === true) {
          this.lstProduct = res.data;
          return;
        }
        this.messageService.add({
          key: 'bc',
          severity: 'error',
          summary: 'Lỗi',
          detail: res.message,
        });
      });
  }
  createImgPath = (serverPath: string, cateId?: number) => {
    if (cateId && cateId === -1) return serverPath;
    return CoreConstants.apiUrl() + `/${serverPath}`;
  };
  openDetailPopup(data: any) {
    this.currentProduct = data;
    this.websiteAPIService.getCartByUserID(Number(this.userData.id)).subscribe((res: any) => {
      this.products = res.data;
      let currentProduct = this.products.filter(x => x.productId == this.currentProduct.id);
      if (currentProduct.length > 0 &&  currentProduct[0].quanity > 0){
        this.quantityCurrentProductInCard = currentProduct[0].quanity;
      }    
    });   
    this.displayDetailPopup = true;
  }
  hideDetailPopup() {
    this.displayDetailPopup = false;
  }
  openFeedbackPopUp(data: any) {
    this.displayFeedbackPopup = true;
    this.currentProduct = data;
    this.feedbackContent = '';
  }
  addToCart(selectedProduct: any) {
    if (selectedProduct.quanity < (this.quantityCurrentProductInCard + this.quantityAddToCart)){
      this.messageService.add({ key:'bc', severity: 'error', summary: 'Lỗi', detail: 'Số lượng sản phẩm có sẵn không đủ (bao gồm số lượng đã có sẵn trong giỏ hàng)'});
      return;
    }
    this.addCart.ProductId = selectedProduct.id;
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
        this.displayDetailPopup = false;
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
  productDetail(selectedProduct: any){
    this.router.navigate(['product-detail/' + selectedProduct.id]);
  }
  updateVisibility(): void {
    this.visible = false;
    setTimeout(() => this.visible = true, 0);
  }
}
