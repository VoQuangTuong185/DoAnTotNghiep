import { Component, Input, OnInit } from '@angular/core';
import { WebsiteAPIService } from '../../data/WebsiteAPI.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { enumData } from '../../core/src/lib/enumData';
import { MatDialog } from '@angular/material/dialog';
import { OrderDetailComponent } from './order-detail/order-detail.component';

@Component({
  selector: 'app-listOrder',
  templateUrl: './listOrder.component.html',
  styleUrls: ['./listOrder.component.scss'],
})
export class ListOrderComponent implements OnInit {
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
      JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1]))
        .id
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
      .getWaitingOrderByUserID(Number(this.userId))
      .subscribe((res: any) => {
        this.orders = res.data;
        this.loading = false;
      });
  }

  getProcessingOrder() {
    this.websiteAPIService
      .getProcessingOrderByUserID(Number(this.userId))
      .subscribe((res: any) => {
        this.orders = res.data;
        this.loading = false;
      });
  }

  getSuccessOrder() {
    this.websiteAPIService
      .getSuccessOrderByUserID(Number(this.userId))
      .subscribe((res: any) => {
        this.orders = res.data;
        this.loading = false;
      });
  }

  getCancelOrder() {
    this.websiteAPIService
      .getCancelOrderByUserID(Number(this.userId))
      .subscribe((res: any) => {
        this.orders = res.data;
        this.loading = false;
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
        if(res.data){
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
          this.getWaitingOrder();
        }
        else {
          this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
        }
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

  successOrder(orderId: string) {
    this.loading = true;
    this.websiteAPIService
      .successOrder(Number(orderId))
      .subscribe((res: any) => {
        if(res.data){
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
          this.getProcessingOrder();
        }
        else {
          this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
        }
      });
  }

  confirmSuccessOrder(orderId: string) {
    this.confirmationService.confirm({
      message: 'Xác nhận hoàn tất đơn hàng?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.successOrder(orderId);
      },
    });
  }
}
