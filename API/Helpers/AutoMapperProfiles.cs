using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        // User
        CreateMap<AppUser, VMember>();
        CreateMap<VMember, AppUser>();
        
        // Photo
        CreateMap<Photo, VPhoto>();
        CreateMap<VPhoto, Photo>();
        
    }
}