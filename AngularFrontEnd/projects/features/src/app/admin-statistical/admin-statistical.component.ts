import { Component } from '@angular/core';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { OrderStatisticalFilter } from '../data/OrderStatisticalFilter';

@Component({
  selector: 'app-admin-statistical',
  templateUrl: './admin-statistical.component.html',
  styleUrls: ['./admin-statistical.component.scss']
})
export class AdminStatisticalComponent {
  selectedRole: any = 'revenue';
  selectedRoleIndex: number = 0;
  roles!: any[];

  selectedTypeOrder: any = 'year';
  selectedOrderIndex: number = 0;
  typeOrders!: any[];

  selectedTypeRevenue: any = 'year';
  selectedRevenueIndex: number = 0;
  typeRevenues!: any[];

  data: any;

  options: any;

  yearOrder: Date = new Date();
  rangeOrder!: Date[];
  dayOrder: Date = new Date();

  revenuesData!: any[];

  constructor(private websiteAPIService : WebsiteAPIService){
    this.roles = [
      { name: 'Doanh thu',  value: 'revenue'},
      { name: 'Đơn hàng',  value: 'order'}];
    this.typeOrders = [
      { name: 'Năm',  value: 'year'},
      { name: 'Ngày',  value: 'day'},
      { name: 'Khoảng thời gian',  value: 'range'}];
    let year = 'year';
    let filter1 = new OrderStatisticalFilter(year, this.yearOrder, this.yearOrder);       
    this.loadDataRevenues(filter1);
  }
  changeRoleUserDisplay(value : any){
    this.selectedRoleIndex = value.index;

    switch(value.index) { 
        case 1:
          this.onGetData(0);
          break;
      } 
   }

   changeOrderDisplay(value : any){
    this.selectedOrderIndex = value.index;
    this.onGetData(value.index);
   }  

   async onGetData(index: number){
    switch(index) { 
      case 0:
        let year = 'year';
        let filter1 = new OrderStatisticalFilter(year, this.yearOrder, this.yearOrder);       
        this.loadDataRevenues(filter1);
        break;
      case 1:
        let day = 'day';
        let filter2 = new OrderStatisticalFilter(day, this.dayOrder, this.yearOrder);    
        this.loadDataRevenues(filter2);
        break;
      case 2:
        let range = 'range';
        let filter3 = new OrderStatisticalFilter(range, this.rangeOrder[0], this.rangeOrder[1]);    
        this.loadDataRevenues(filter3);
        break;
    } 
   }
   
   changeRevenueDisplay(value : any){
    this.selectedRevenueIndex = value.index;
    switch(value.index) { 

      } 
   }  
   ngOnInit() {

  }
  loadDataRevenues(filter : OrderStatisticalFilter){
    this.websiteAPIService.getOrderStatisticalsByFilter(filter).subscribe((res:any) => {
      var result = res.data.map(function(a: { count: any; }) {return a.count;});
      this.changeData(result);
    });
  }
  changeData(data: number[]) {
    let changedData = {
        labels: ['Huỷ', 'Chờ xác nhận', 'Đang xử lý', 'Hoàn tất'],
        datasets: [
          {
            data: data,
            backgroundColor: [ "#FF0000", "#f441c4", "#4195f4", "#50f442", ],
            hoverBackgroundColor: [ "#FF0000","#f441c4", "#4195f4", "#50f442", ]
          }]
    }
    this.data = Object.assign({}, changedData);
  }
}
