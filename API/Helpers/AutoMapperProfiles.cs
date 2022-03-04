using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        // User
        CreateMap<AppUser, MemberDto>();
        CreateMap<MemberDto, AppUser>();
        
        // Photo
        CreateMap<Photo, PhotoDto>();
        
    }
}