import { Component, HostBinding, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { MemberDto, User } from 'src/app/_entities/entities';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/membersService/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  member: MemberDto
  user: User
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private accountService: AccountService, private memberService: MembersService, private $toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getById(this.user.id).subscribe(member => {
      this.member = member;
    })
  }

  updateMember() {
    this.member.gender = 'female';
    this.memberService.update(this.member).subscribe(()=>{
      this.$toastr.success('Profifle updated Succesfully');
      this.editForm.reset(this.member);
    })
  }

}
