import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { MemberDto } from 'src/app/_entities/entities';
import { MembersService } from 'src/app/_services/membersService/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members$: Observable<MemberDto[]>

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.members$ = this.memberService.getAll();
  }
}
