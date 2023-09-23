import { Component, OnInit } from '@angular/core';
import { enumData } from '../core/src/lib/enumData';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
})
export class OrderComponent implements OnInit {
  enumData = enumData;
  currentOrder = enumData.statusOrder.wait.value;
  constructor() {}

  ngOnInit() {
  }

  handleChangeViewOrder(event: any) {
    this.currentOrder = event.index;
  }
}
