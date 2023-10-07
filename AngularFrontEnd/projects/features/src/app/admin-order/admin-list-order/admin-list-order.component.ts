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
  constructor(
    private websiteAPIService: WebsiteAPIService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialog: MatDialog
  ) {
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
    this.websiteAPIService
      .getWaitingOrder()
      .subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  getProcessingOrder() {
    this.websiteAPIService
      .getProcessingOrder()
      .subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  getSuccessOrder() {
    this.websiteAPIService
      .getSuccessOrder()
      .subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  getCancelOrder() {
    this.websiteAPIService
      .getCancelOrder()
      .subscribe((res: any) => {
        if(res.isSuccess === true){
          this.orders = res.data;
          this.loading = false;
        }
      });
  }

  openModalOrderDetail(order: any) {
    this.dialog
      .open(OrderDetailComponent, { disableClose: false, data: order })
      .afterClosed()
      .subscribe(() => {});
  }

  cancelOrder(orderId: string) {
    this.loading = true;
    this.websiteAPIService
      .cancelOrder(Number(orderId))
      .subscribe((res: any) => {
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
            detail: 'Huỷ order successfully',
          });
          return
        }
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
      });
  }

  confirmOrder(orderId: string) {
    this.loading = true;
    this.websiteAPIService
      .confirmOrder(Number(orderId))
      .subscribe((res: any) => {
        if(res.isSuccess === true){
          this.getWaitingOrder();
          this.messageService.add({
            key: 'bc',
            severity: 'info',
            summary: 'Confirmed',
            detail: 'Xác nhận order successfully',
          });
          return
        }
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
      });
  }

  confirmCancelOrder(orderId: string) {
    this.confirmationService.confirm({
      message: 'Do you want to cancel this order?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.cancelOrder(orderId);
      },
    });
  }

  confirmProcessingOrder(orderId: string) {
    this.confirmationService.confirm({
      message: 'Do you want to confirm?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.confirmOrder(orderId);
      },
    });
  }

  successOrder(orderId: string) {
    this.loading = true;
    this.websiteAPIService
      .successOrder(Number(orderId))
      .subscribe((res: any) => {
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
