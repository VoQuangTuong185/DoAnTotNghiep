import { Component, Input, OnInit } from '@angular/core';
import { WebsiteAPIService } from '../../data/WebsiteAPI.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { enumData } from '../../core/src/lib/enumData';
import { MatDialog } from '@angular/material/dialog';
import { OrderDetailComponent } from './order-detail/order-detail.component';
import { FormArray, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Feedback } from '../../data/Feedback.model';
import { FeedbackDTO } from '../../data/FeedbackDTO.model';

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
  displayFeedbackPopup :boolean = false;
  feedbackForm: FormGroup;
  lstProduct: any[] = [];
  listFeedback: any[] = [];
  listSubmitFeedback: FeedbackDTO[] = [];
  currentOrderId!: number;
  constructor(
    private websiteAPIService: WebsiteAPIService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private dialog: MatDialog,
    private formBuilder: FormBuilder
  ) {
    this.userId = Number(JSON.parse(window.atob(localStorage.getItem('authToken')!.split('.')[1])).id);
    this.feedbackForm = this.createEmptyListFeedback();
  }
  get feedbacks(){
    return this.feedbackForm?.get('feedbacks') as FormArray;
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
      .cancelOrderUser(Number(orderId))
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
      .successOrderUser(Number(orderId))
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
  openFeedback(order: any) {
    this.displayFeedbackPopup = true;
    this.currentOrderId = order.id;
    this.getAllProductByOrder(order.id);
  }
  private createEmptyListFeedback(){
    return this.formBuilder.group({
      feedbacks: this.formBuilder.array([])
    });
  }
  private createExistedListFeedback(order: any){
    return this.formBuilder.group({
      ProductName : [order.ProductName, Validators.required],
      Votes : [order.Votes, Validators.required],
      Comments : [order.Comments, ],
      ProductId : [order.ProductId, ],
    });
  }
  submitFeedback(){
    this.listFeedback = this.feedbacks.value;
    this.listFeedback.forEach(x => {
      let feedback = new FeedbackDTO();
      feedback.UserId = this.userId;
      feedback.OrderId = this.currentOrderId;
      feedback.Votes = x.Votes;
      feedback.Comments = x.Comments;
      feedback.ProductId = x.ProductId;
      this.listSubmitFeedback.push(feedback);
    });
    this.websiteAPIService.createFeedback(this.listSubmitFeedback).subscribe((res: any) => {
      if(res.data){
        this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: res.message, life: 3000});
        this.getWaitingOrder();
      }
      else {
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: res.message});
      }
    }); 
    this.feedbackForm = this.createEmptyListFeedback();
    this.displayFeedbackPopup = false;
  }
  hideFeedbackPopup(){
    this.displayFeedbackPopup = false;
    this.feedbackForm = this.createEmptyListFeedback();
  }
  getAllProductByOrder(orderId: number) {
    this.websiteAPIService.getAllProductByOrderIDUser(orderId).subscribe((res: any) => {
      if (res.isSuccess == true) {
        this.lstProduct = res.data;   
        this.lstProduct.forEach(x => {
          let feedback = new Feedback();
          feedback.ProductName = x.productName;
          feedback.Votes = x.votes;
          feedback.Comments = x.comments;
          feedback.ProductId = x.productId;
          this.feedbacks.push(this.createExistedListFeedback(feedback));
        });
      }
    }); 
  }
}
