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
  rangeOrder: Date[] = [new Date(new Date().getFullYear(), new Date().getMonth(), 1), new Date()];
  dayOrder: Date = new Date(); 

  yearRevenue: Date = new Date();
  rangeRevenue: Date[] = [new Date(new Date().getFullYear(), new Date().getMonth(), 1), new Date()];

  revenuesData!: any[];

  dataRevenue: any;

  optionsRevenue: any;

  constructor(private websiteAPIService : WebsiteAPIService){
    this.roles = [
      { name: 'Doanh thu',  value: 'revenue'},
      { name: 'Đơn hàng',  value: 'order'}];
    this.typeOrders = [
      { name: 'Năm',  value: 'year'},
      { name: 'Ngày',  value: 'day'},
      { name: 'Khoảng thời gian',  value: 'range'}];
      this.typeRevenues = [
        { name: 'Năm',  value: 'year'},
        { name: 'Khoảng thời gian',  value: 'range'}];
    let year = 'year';
    let filter1 = new OrderStatisticalFilter(year, this.subtractDays(1), this.yearOrder);
    let filter2 = new OrderStatisticalFilter(year, this.subtractDays(1), this.yearRevenue);       
    this.loadDataOrders(filter1);
    this.loadDataRevenues(filter2);
  }
  changeRoleUserDisplay(value : any){
    this.selectedRoleIndex = value.index;

    switch(value.index) { 
        case 1:
          this.onGetDataOrders(0, true);
          break;
      } 
   }

   changeOrderDisplay(value : any){
    this.selectedOrderIndex = value.index;
    this.onGetDataOrders(value.index, true);
   }  

   async onGetDataOrders(index: number, isFirstLoad: boolean){
    switch(index) { 
      case 0:
        let year = 'year';
        let filter1;
        if (!isFirstLoad){
          filter1 = new OrderStatisticalFilter(year, this.yearOrder, this.yearOrder);       
        }
        else {
          filter1 = new OrderStatisticalFilter(year, this.subtractDays(1), this.yearOrder);
        }
        this.loadDataOrders(filter1);
        break;
      case 1:
        let day = 'day';
        console.log(this.dayOrder)
        if (this.dayOrder == undefined){
          this.changeDataOrders([],[]);
        }       
        let filter2 = new OrderStatisticalFilter(day, this.addHours(this.dayOrder, 7), this.yearOrder);  
        console.log(filter2) 
        this.loadDataOrders(filter2);
        break;
      case 2:
        let range = 'range';
        let filter3 = new OrderStatisticalFilter(range, this.addHours(this.rangeOrder[0], 7), this.addHours(this.rangeOrder[1], 7));
        this.loadDataOrders(filter3);
        break;
    } 
   }
   
   changeRevenueDisplay(value : any){
    this.selectedRevenueIndex = value.index;
    this.onGetDataRevenue(value.index);
   }  

   async onGetDataRevenue(index: number){
    switch(index) { 
      case 0:
        let year = 'year';
        let filter1 = new OrderStatisticalFilter(year, this.yearRevenue, this.yearRevenue);       
        this.loadDataRevenues(filter1);
        break;
      case 1:
        let range = 'range';
        let filter3 = new OrderStatisticalFilter(range, this.addHours(this.rangeRevenue[0], 7), this.addHours(this.rangeRevenue[1], 7));    
        this.loadDataRevenues(filter3);
        break;
    } 
   }
   
   
   ngOnInit() {

  }
  loadDataOrders(filter : OrderStatisticalFilter){
    let count;
    let status;
    this.websiteAPIService.getOrderStatisticalsByFilter(filter).subscribe((res:any) => {
      count = res.data.map(function(a: { count: any; }) {return a.count;});
      status = res.data.map(function(a: { status: any; }) {return a.status;});
      this.changeDataOrders(count, status);
    });
  }
  changeDataOrders(data: number[], label: string[]) {
    let dataTable = data == null ? [] : data;
    let changedData = {
        labels: label,
        datasets: [
          {
            data: dataTable,
            backgroundColor: [ "#4195f4", "#50f442", "#FF0000",  "#f441c4" ],
            hoverBackgroundColor: [ "#4195f4", "#50f442", "#FF0000",  "#f441c4" ]
          }]
    }
    this.data = Object.assign({}, changedData);
  }

  loadDataRevenues(filter : OrderStatisticalFilter){
    let month;
    let totalMoney;
    this.websiteAPIService.getRevenueStatisticalsByFilter(filter).subscribe((res:any) => {
      month = res.data.map(function(a: { month: any; }) {return a.month;});
      totalMoney = res.data.map(function(a: { totalMoney: any; }) {return a.totalMoney;});
      this.changeDataRevenues(totalMoney, month);
    });
  }

  changeDataRevenues(data: number[], label: string[]){
    let dataTable = data == null ? [] : data;
    this.dataRevenue = {
        labels: label,
        datasets: [
            {
                label: 'Thống kê doanh thu (VND)',
                backgroundColor: "#4195f4",
                borderColor: "#4195f4",
                data: dataTable
            }
        ]
    };

    this.optionsRevenue = {
        maintainAspectRatio: false,
        aspectRatio: 0.8,
        plugins: {
            legend: {
                labels: {
                    color: "#4195f4"
                }
            }
        },
        scales: {
            x: {
                ticks: {
                    color: "#686868",
                    font: {
                        weight: 400
                    }
                },
                grid: {
                    color: "#bebebe",
                    drawBorder: false
                }
            },
            y: {
                ticks: {
                    color: "#686868"
                },
                grid: {
                    color: "#bebebe",
                    drawBorder: false
                }
            }

        }
    };
  }
  subtractDays(days: number): Date {
    let date = new Date();
    date.setFullYear(date.getFullYear() - days);
    return date;
  } 
  addHours(date: Date ,hours: number): Date {
    date.setHours(date.getHours() + hours);
    return date;
  } 
}
