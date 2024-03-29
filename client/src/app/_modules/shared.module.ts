import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { TabsModule} from 'ngx-bootstrap/tabs';
import { NgxSpinnerModule } from "ngx-spinner";
import { FileUploadModule } from 'ng2-file-upload';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-top-right'
    }),
    TabsModule.forRoot(),
    NgxSpinnerModule,
    FileUploadModule
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    NgxSpinnerModule,
    FileUploadModule
  ]

})
export class SharedModule { }
