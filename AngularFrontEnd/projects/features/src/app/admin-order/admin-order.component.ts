import { Component, OnInit } from '@angular/core';
import { enumData } from '../core/src/lib/enumData';

@Component({
  selector: 'app-admin-order',
  templateUrl: './admin-order.component.html',
  styleUrls: ['./admin-order.component.scss'],
})
export class AdminOrderComponent implements OnInit {
  enumData = enumData;
  currentOrder = enumData.statusOrder.wait.value;
  constructor() {}

  ngOnInit() {
  }

  handleChangeViewOrder(event: any) {
    this.currentOrder = event.index;
  }
}
