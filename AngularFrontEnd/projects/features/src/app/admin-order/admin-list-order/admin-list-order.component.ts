import { Component, Input, OnInit } from '@angular/core';
import { WebsiteAPIService } from '../../data/WebsiteAPI.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { enumData } from '../../core/src/lib/enumData';
import { MatDialog } from '@angular/material/dialog';
import { OrderDetailComponent } from '../../order/listOrder/order-detail/order-detail.component';

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
  constructor(
    private websiteAPIService: WebsiteAPIService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialog: MatDialog
  ) {
    this.ordersDataCols = [
      { header : 'STT', field : 'id', width:5, type:'stt'},
      { header : 'Tên khách hàng', field : 'customerName', width:20, type:'string'},
      { header : 'Địa chỉ mail', field : 'email', width:20, type:'string'},
      { header : 'Số sản phẩm', field : 'productCount', width:5, type:'number'},
      { header : 'Thời gian đặt hàng', field : 'orderDate', width:10, type:'date'},
      { header : 'Phương thức nhận hàng', field : 'payment', width:10, type:'string'},
      { header : 'Tổng tiền', field : 'totalBill', width:25, type:'money'},
      { header : 'Thao tác', field : 'statusOrderCode', width:25, type:'button'},
      ];
    this.userId = Number(
      JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1])).id
    );
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
    this.websiteAPIService.cancelOrder(Number(orderId)).subscribe((res: any) => {
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
    this.websiteAPIService.confirmOrder(Number(orderId)).subscribe((res: any) => {
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
    this.websiteAPIService.successOrder(Number(orderId)).subscribe((res: any) => {
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
      message: 'Do you want to confirm?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.successOrder(orderId);
      },
    });
  }
}
