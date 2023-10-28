import { Component, OnInit } from '@angular/core';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { User } from '../data/User.model';
import { CoreConstants } from '../core/src/lib/core.constant';
import { CartDTO } from '../data/cartDTO.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddressService } from '../data/Address.service';
import { UpdateCart } from '../data/UpdateCart.model';
import { CheckValidEmailService } from '../data/CheckValidEmailService';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
})
export class CartComponent implements OnInit {
  products: any[] = [];
  showConfirmOrder: boolean = false;
  currentUser: any;
  userData = new User();
  selectedCategory: any = null;
  editUserForm!: FormGroup;
  provices: any[];
  districts: any[];
  wards: any[];
  provinceSelected: any;
  districtSelected: any;
  wardSelected: any;
  existedProvince: any;
  existedDistrict: any;
  existedWard: any;
  selectedPayment: string = '';
  userId!: number;
  methods: any[] = [
    { name: 'Thanh toán khi nhân hàng', key: 'A' },
    { name: 'Thanh toán qua ngân hàng', key: 'M' },
    { name: 'Thanh toán qua momo', key: 'P' },
  ];
  selectedAll: boolean = false;
  constructor(
    private websiteAPIService: WebsiteAPIService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private formBuilder: FormBuilder,
    private addressService: AddressService,
    private checkValidEmailService : CheckValidEmailService
  ) {
    this.provices = [];
    this.districts = [];
    this.wards = [];
    if (
      localStorage.getItem('authToken') != null &&
      localStorage.getItem('authToken') != 'null'
    ) {
      this.userData = JSON.parse(
        window.atob(localStorage.getItem('authToken')!.split('.')[1])
      );
    }
    this.loadDataUser();
    this.selectedPayment = this.methods[0].key;
  }
  ngOnInit() {
    this.getCartByUserID();
    this.getAllProvices();
  }
  createExistedUserForm() {
    return this.formBuilder.group({
      UserId: [this.currentUser.id],
      Name: [this.currentUser.name, [Validators.required]],
      Email: [{value: this.currentUser.email, disabled: true}, [Validators.required]],
      TelNum: [this.currentUser.telNum,  Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(10)])],
      Provinces: [this.existedProvince[0], Validators.required],
      Districts: [this.existedDistrict[0], Validators.required],
      Wards: [this.existedWard[0], Validators.required],
      Streets: [this.currentUser.streets, Validators.required],
      Payment: ['A', Validators.required],
    });
  }
  getCartByUserID() {
    this.websiteAPIService
      .getCartByUserID(Number(this.userData.id))
      .subscribe((res: any) => {
        this.products = res.data;
      });
  }
  getTotalMoney() {
    let totalMoney = 0;
    this.products.forEach((p) => {
      totalMoney = totalMoney + p.quanity * p.price - (p.quanity * p.price * p.discount) / 100;
    });
    return totalMoney;
  }
  showViewConfirm() {
    this.existedProvince = this.provices.filter(
      (x) => x.code == this.currentUser.provinceCode
    );
    this.provinceSelected = this.existedProvince[0];

    this.existedDistrict = this.districts.filter(
      (x) => x.code == this.currentUser.districtCode
    );
    this.districtSelected = this.existedDistrict[0];

    this.existedWard = this.wards.filter(
      (x) => x.code == this.currentUser.wardCode
    );
    this.wardSelected = this.existedWard[0];

    this.editUserForm = this.createExistedUserForm();
    this.showConfirmOrder = true;
  }
  loadDataUser() {
    this.userId = Number(JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1])).id);
    this.websiteAPIService.getInfoUser(this.userId).subscribe((res: any) => {
      if (res.isSuccess) {
        this.currentUser = res.data;
        this.getDistrictsOfProvice2(res.data.provinceCode);
        this.getWardsOfDistrict2(res.data.districtCode);
      } else {
        this.messageService.add({ key: 'bc', severity: 'info', summary: 'Thông tin', detail: res.message });
      }
    });
  }
  closeConfirmOrder() {
    this.showConfirmOrder = false;
  }
  createImgPath = (serverPath: string) => {
    return CoreConstants.apiUrl() + `/${serverPath}`;
  };
  deleteCartItem(selectedProduct: CartDTO) {
    this.confirmationService.confirm({
      message: 'Xác nhận xoá sản phẩm ra khỏi giỏ hàng?',
      header: 'Xác nhận',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.websiteAPIService
          .inActiveCart(selectedProduct)
          .subscribe((res: any) => {
            if (res.data) {
              this.messageService.add({ key: 'bc', severity: 'success', summary: 'Thành công', detail: res.message, life: 3000});
              this.getCartByUserID();
            } else {
              this.messageService.add({ key: 'bc', severity: 'error', summary: 'Lỗi', detail: res.message });
            }
          });
      },
    });
  }
  getAllProvices(): void {
    this.addressService.getProvices().pipe().subscribe((data) => { this.provices = data; });
  }
  getDistrictsOfProvice2(provinceCode: number): void {
    this.wards = [];
    this.addressService
      .getDistrictsOfProvince(provinceCode)
      .subscribe((data) => {
        this.districts = data.districts;
      });
  }
  getWardsOfDistrict2(districtCode: number): void {
    this.addressService.getWardsOfDistrict(districtCode).subscribe((data) => {
      this.wards = data.wards;
    });
  }
  handleChange() {
    this.editUserForm.get('Provinces')?.valueChanges.subscribe((provices) => {
      this.provinceSelected = provices;
      this.getDistrictsOfProvice2(this.provinceSelected.code);
    });
    this.editUserForm.get('Districts')?.valueChanges.subscribe((districts) => {
      this.districtSelected = districts;
      this.getWardsOfDistrict2(this.districtSelected.code);
    });
    this.editUserForm.get('Wards')?.valueChanges.subscribe((userWard) => {
      this.wardSelected = userWard;
    });
  }
  changeValuePayment(event: any) {
    this.selectedPayment = event;
  }
  updateCart(value: any, productId: number, oldQuantity: number){
    let updateCart = new UpdateCart(Number(this.userData.id), productId, value);
    updateCart.UserId = Number(this.userData.id);
    updateCart.ProductId = productId;
    updateCart.Quantity = value;
    this.websiteAPIService.updateCart(updateCart).subscribe((res: any) => {
      if (res.data) {
        this.messageService.add({ key: 'bc', severity: 'success', summary: 'Thành công', detail: res.message});
        this.getCartByUserID();
      } else {
        this.messageService.add({ key: 'bc', severity: 'error', summary: 'Lỗi', detail: res.message });
        this.products[this.products.findIndex(x => x.productId == productId)].quanity = oldQuantity -1;
      }
    });
  }
  confirmOrder() {
    if(this.editUserForm.invalid){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Hãy nhập các thông tin bắt buộc để cập nhật!',
      });
      return;
    }
    if (!this.checkValidEmailService.isValidEmail(this.editUserForm.controls['Email'].value)){
      this.messageService.add({
        key: 'bc',
        severity: 'error',
        summary: 'Lỗi',
        detail: 'Địa chỉ email bạn nhập không hợp lệ, hãy thử lại!',
      });
      return;
    }
    this.confirmationService.confirm({
      message: 'Are you sure to confirm order ?',
      header: 'Xác nhận',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.editUserForm.controls['Provinces'].setValue(this.provinceSelected.name);
        this.editUserForm.controls['Districts'].setValue(this.districtSelected.name);
        this.editUserForm.controls['Wards'].setValue(this.wardSelected.name);
        this.editUserForm.controls['Payment'].setValue(this.selectedPayment); 
        this.websiteAPIService
          .createOrder(this.editUserForm.getRawValue())
          .subscribe((res: any) => {
            if (res.data) {
              this.showConfirmOrder = false;
              this.websiteAPIService
                .deleteAllCartAfterOrder(Number(this.userData.id))
                .subscribe((res: any) => {
                  if (res.data) {
                    this.messageService.add({ key: 'bc', severity: 'success', summary: 'Thành công', detail: res.message});                
                  } else {
                    this.messageService.add({ key: 'bc', severity: 'error', summary: 'Lỗi', detail: res.message });
                  }                
                });
            } else {
              this.messageService.add({ key: 'bc', severity: 'error', summary: 'Lỗi', detail: res.message});
            }
            this.getCartByUserID();
            this.showConfirmOrder = false;
          });
      },
    });    
  }
}
