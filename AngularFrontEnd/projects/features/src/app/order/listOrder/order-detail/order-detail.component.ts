import { Component, OnInit, Optional, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { WebsiteAPIService } from '../../../data/WebsiteAPI.service';
import { CoreConstants } from '../../../core/src/lib/core.constant';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss'],
})
export class OrderDetailComponent implements OnInit {
  lstProduct: any[] = [];
  constructor(
    private websiteAPIService: WebsiteAPIService,
    private dialogRef: MatDialogRef<OrderDetailComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit() {
    this.getAllProductByOrder();
  }

  getAllProductByOrder() {
    this.websiteAPIService
      .getAllProductByOrderID(Number(this.data.id))
      .subscribe((res: any) => {
        if (res.isSuccess === true) {
          this.lstProduct = res.data;
        }
      });
  }
  
  getTotalMoney(price: number, quantity: number, discount: number) {
    return price * quantity * ((100 - discount) / 100);
  }

  createImgPath = (serverPath: string) => {
    return CoreConstants.apiUrl() + `/${serverPath}`;
  };

  closeDialog() {
    this.dialogRef.close(1);
  }
}
