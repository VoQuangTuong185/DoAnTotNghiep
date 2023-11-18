import { Component } from '@angular/core';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { VIPDto } from '../data/VIP.model';

@Component({
  selector: 'app-vip',
  templateUrl: './vip.component.html',
  styleUrls: ['./vip.component.scss']
})
export class VipComponent {
  dataVIPs: VIPDto[] = [];
  constructor(private websiteAPIService : WebsiteAPIService) {

  }
  ngOnInit() {
    this.loadDataAllVIPs();
  }
  loadDataAllVIPs(){
    this.websiteAPIService.getAllVIP().subscribe((res:any) => {
      this.dataVIPs = res.data;  
    });
  }
}
