import { Component, OnInit } from '@angular/core';
import { MemberDto } from 'src/app/_entities/entities';
import { MembersService } from 'src/app/_services/membersService/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members: MemberDto[]

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getAll().subscribe(members => {
      this.members = members;
    })
  }
}
