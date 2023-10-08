import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';
import { RegisterConstant } from '../../RegisterCourse.constant';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-upload-image-category',
  templateUrl: './upload-image-category.component.html',
  styleUrls: ['./upload-image-category.component.scss']
})
export class UploadImageCategoryComponent {
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
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this.http.post(RegisterConstant.libraryApiUrlCategory() + 'upload-course-image', formData, {reportProgress: true, observe: 'events'})
      .subscribe({
        next: (event) => {
        if (event.type === HttpEventType.UploadProgress){
          if(event?.loaded && event?.total ) {
            this.progress = Math.round(100 * event.loaded / event.total)
          }
        }        
        else if (event.type === HttpEventType.Response) {
          this.messageService.add({key: 'bc', severity:'success', summary: 'Thành công', detail: 'Tải lên ảnh thành công!'});
          this.onUploadFinished.emit(event.body);
        }
      },
      error: (err: HttpErrorResponse) => {
        this.messageService.add({key: 'bc', severity:'error', summary: 'Lỗi', detail: 'Tải lên ảnh thất bại!'});
      }
    });
  }
}
