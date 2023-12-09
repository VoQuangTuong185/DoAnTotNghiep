import { Component, Input, OnInit } from '@angular/core';
import { WebsiteAPIService } from '../../data/WebsiteAPI.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { enumData } from '../../core/src/lib/enumData';
import { MatDialog } from '@angular/material/dialog';
import { OrderDetailComponent } from '../../order/listOrder/order-detail/order-detail.component';
import { OrderAdminModel } from '../../data/OrderAdminModel';
import jwt_decode, { JwtPayload } from 'jwt-decode'
import { User } from '../../data/User.model';

@Component({
  selector: 'app-admin-list-order',
  templateUrl: './admin-list-order.component.html',
  styleUrls: ['./admin-list-order.component.scss'],
})
export class AdminListOrderComponent implements OnInit {
  orders: any[] = [];
  userId!: number;
  enumData = enumData;
  loading: boolean = false;
  @Input() statusOrderCode: string | undefined;
  ordersDataCols:any[] = [];
  first:number = 10;
  rows:number = 10;
  userData = new User();
  constructor(
    private websiteAPIService: WebsiteAPIService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialog: MatDialog
  ) {
    this.ordersDataCols = [
      { header : 'STT', field : 'id', width:5, type:'stt'},
      { header : 'Tên khách hàng', field : 'customerName', width:25, type:'string'},
      { header : 'Địa chỉ mail', field : 'email', width:20, type:'string'},
      { header : 'Số sản phẩm', field : 'productCount', width:5, type:'number'},
      { header : 'Thời gian đặt hàng', field : 'orderDate', width:10, type:'date'},
      { header : 'Phương thức nhận hàng', field : 'payment', width:10, type:'string'},
      { header : 'VIP', field : 'discountVIP', width:10, type:'percent'},
      { header : 'Tổng tiền', field : 'totalBill', width:10, type:'money'},
      { header : 'Thao tác', field : 'statusOrderCode', width:25, type:'button'},
      ];
    this.userData = jwt_decode(localStorage.getItem('authToken')!.replace(/-/g, "+").replace(/_/g, "/"));
    this.userId = this.userData.id;
  }

  ngOnInit() {
    if (this.statusOrderCode === this.enumData.statusOrder.wait.code) {
      this.getWaitingOrder();
      return;
    }
    if (this.statusOrderCode === this.enumData.statusOrder.processing.code) {
      this.getProcessingOrder();
      return;
    }
    if (this.statusOrderCode === this.enumData.statusOrder.success.code) {
      this.getSuccessOrder();
      return;
    }
    if (this.statusOrderCode === this.enumData.statusOrder.cancel.code) {
      this.getCancelOrder();
      return;
    }
  }
  getWaitingOrder() {
    this.websiteAPIService.getWaitingOrder().subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  getProcessingOrder() {
    this.websiteAPIService.getProcessingOrder().subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  getSuccessOrder() {
    this.websiteAPIService.getSuccessOrder().subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  getCancelOrder() {
    this.websiteAPIService.getCancelOrder().subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  openModalOrderDetail(order: any) {
    this.dialog.open(OrderDetailComponent, { disableClose: false, data: order }).afterClosed().subscribe(() => {});
  }

  cancelOrder(orderId: string) {
    this.loading = true;
    let adminOrder = new OrderAdminModel();
    adminOrder.orderId = Number(orderId);
    adminOrder.updateBy = Number(this.userId);
    this.websiteAPIService.cancelOrderAdmin(adminOrder).subscribe((res: any) => {
        if(res.isSuccess === true){
          if(this.statusOrderCode === enumData.statusOrder.wait.code){ 
            this.getWaitingOrder()
          }
          else {
            this.getProcessingOrder()
          }
          this.messageService.add({
            key: 'bc',
            severity: 'info',
            summary: 'Confirmed',
            detail: 'Huỷ đơn hàng',
          });
          return
        }
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
      });
  }

  confirmOrder(orderId: string) {
    this.loading = true;
    let adminOrder = new OrderAdminModel();
    adminOrder.orderId = Number(orderId);
    adminOrder.updateBy = Number(this.userId);
    this.websiteAPIService.confirmOrderAdmin(adminOrder).subscribe((res: any) => {
        if(res.isSuccess === true){
          this.getWaitingOrder();
          this.messageService.add({
            key: 'bc',
            severity: 'info',
            summary: 'Confirmed',
            detail: 'Xác nhận đơn hàng thành công',
          });
          return
        }
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
      });
  }

  confirmCancelOrder(orderId: string) {
    this.confirmationService.confirm({
      message: 'Xác nhận huỷ đơn hàng?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.cancelOrder(orderId);
      },
    });
  }

  confirmProcessingOrder(orderId: string) {
    this.confirmationService.confirm({
      message: 'Xác nhận đang xử lý đơn hàng?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.confirmOrder(orderId);
      },
    });
  }

  successOrder(orderId: string) {
    this.loading = true;
    let adminOrder = new OrderAdminModel();
    adminOrder.orderId = Number(orderId);
    adminOrder.updateBy = Number(this.userId);
    this.websiteAPIService.successOrderAdmin(adminOrder).subscribe((res: any) => {
        if(res.isSuccess === true){
          this.getProcessingOrder();
          this.messageService.add({
            key: 'bc',
            severity: 'info',
            summary: 'Confirmed',
            detail: 'Thành công',
          });
          return
        }
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
      });
  }

  confirmSuccessOrder(orderId: string) {
    this.confirmationService.confirm({
      message: 'Xác nhận hoàn tất đơn hàng này?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.successOrder(orderId);
      },
    });
  }
}
