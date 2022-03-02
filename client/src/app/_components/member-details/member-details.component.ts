import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { MemberDto } from 'src/app/_entities/entities';
import { MembersService } from 'src/app/_services/membersService/members.service';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  member: MemberDto

  constructor(private memberService: MembersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMember();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    if(this.member.photos){
      for(const photo of this.member.photos)
      {
        imageUrls.push({
          small: photo?.url,
          medium: photo?.url,
          big: photo?.url
        })
      }
    }
      
    return imageUrls
  }

  loadMember() {
    this.memberService.getById(Number(this.route.snapshot.paramMap.get('id'))).subscribe(member => {
      this.member = member;
      this.galleryImages = this.getImages();
    })
  }

}
