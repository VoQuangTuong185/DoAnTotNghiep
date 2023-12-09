import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';
import { Constant } from '../../data/WebsiteApi.constant';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.scss']
})
export class UploadImageComponent {
  constructor(
    private http: HttpClient,
    private messageService: MessageService){}
  progress!: number;
  message!: string;
  @Output() public onUploadFinished = new EventEmitter();
  uploadFile = (files : any) => {
    if (files.length === 0) {
      return;
    }
    let temporaryFile = <File>files[0];

    let fileToUpload = new File([<File>files[0]], this.makeString() + "." + temporaryFile.name.split(".", 2)[1]);
    console.log(fileToUpload)
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.http.post(Constant.libraryApiUrlAdmin() + 'upload-course-image', formData, {reportProgress: true, observe: 'events'})
      .subscribe({
        next: (event) => {
        if (event.type === HttpEventType.UploadProgress){
          if(event?.loaded && event?.total ) {
            this.progress = Math.round(100 * event.loaded / event.total)
          }
        }        
        else if (event.type === HttpEventType.Response) {
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: 'Tải lên ảnh thành công!'});
          this.http.post(Constant.libraryApiUrlUser() + 'upload-course-image', formData, {reportProgress: true, observe: 'events'})
          .subscribe({
            next: (event) => {
            if (event.type === HttpEventType.UploadProgress){
              if(event?.loaded && event?.total ) {
                this.progress = Math.round(100 * event.loaded / event.total)
              }
            }    
          }
        });
          this.onUploadFinished.emit(event.body);
        }
      },
      error: (err: HttpErrorResponse) => {
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Tải lên ảnh thất bại!'});
      }
    });
  }
  makeString(): string {
    let outString: string = '';
    let inOptions: string = 'abcdefghijklmnopqrstuvwxyz0123456789';
    for (let i = 0; i < 32; i++) {
      outString += inOptions.charAt(Math.floor(Math.random() * inOptions.length));
    }
    return outString;
  }
  result: string = this.makeString();
}
