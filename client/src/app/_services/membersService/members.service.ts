import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MemberDto } from 'src/app/_entities/entities';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private $http: HttpClient) { }

  getAll() {
    return this.$http.get<MemberDto[]>(this.baseUrl + 'users')
  }

  getById(id: number) {
    return this.$http.get<MemberDto>(this.baseUrl + 'users/' + id)
  }

  update(member: MemberDto) {
    return this.$http.put(this.baseUrl + 'users',member);
  }

  setMainPhoto(photoId: number) {
    return this.$http.put(this.baseUrl + 'users/set-main-photo/' + photoId,{});
  }

  deletePhoto(photoId: number) {
    return this.$http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
