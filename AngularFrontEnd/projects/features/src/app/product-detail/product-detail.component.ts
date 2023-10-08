import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { WebsiteAPIService } from '../data/WebsiteAPI.service';
import { PhotoService } from '../data/photoservice';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  existedProductId!: number;
  images: any[] = [];
  responsiveOptions: any[] = [];
  @Input() value: unknown
@Output() valueChange = new EventEmitter<unknown>();
  constructor(
    private router: Router,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private websiteAPIService: WebsiteAPIService,
    private ActiveRoute : ActivatedRoute,
    private photoService: PhotoService
  ){
    this.ActiveRoute.params.subscribe((params) =>{
      this.existedProductId = params['id'];
    });
  }
  ngOnInit(): void{
    this.getExistedProduct();
    this.photoService.getImages().then((images) => (this.images = images));
      this.responsiveOptions = [
          {
              breakpoint: '1024px',
              numVisible: 5
          },
          {
              breakpoint: '768px',
              numVisible: 3
          },
          {
              breakpoint: '560px',
              numVisible: 1
          }
      ];
  }
  getExistedProduct(){
    this.websiteAPIService.getExistedProduct(this.existedProductId).subscribe((result : any) => {
      var data = result.data;
    });
  }
}
