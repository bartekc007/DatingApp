using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.BusinessLogics;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Repositories;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public UsersController(IUserBusinessLogic users, IMapper mapper, IPhotoService photoService, IPhotosBusinessLogic _photos )
        {
            _mapper = mapper;
            _userBusinessLogic = users;
            _photoService = photoService;
            _photoBusinessLogic = _photos;
        }
        private readonly IUserBusinessLogic _userBusinessLogic;
        private readonly IPhotosBusinessLogic _photoBusinessLogic;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VMember>>> GetAll()
        {
            var result = await _userBusinessLogic.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult<VMember>> GetById(int id)
        {
            var result = await _userBusinessLogic.GetById(id);
            return result;
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMember vMember)
        {
            var username = User.GetUsername();
            if (!string.IsNullOrEmpty(username))
            {
                var user = AppUser.CopyFrom(vMember);

                if(await _userBusinessLogic.Update(user)==1)
                    return NoContent();
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<VPhoto>> AddPhoto(IFormFile file)
        {
            var user = await _userBusinessLogic.GetByUserName(User.GetUsername());

            var result = await _photoService.UploadImageAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                AppUserId = user.Id
            };

            // Check if user has any photo.
            var hasAnyPhoto = await _photoBusinessLogic.HasUserAnyPhotos(user.Id);
            if(hasAnyPhoto)
                photo.IsMain = true;
            else 
                photo.IsMain = false;

            // Add photo info to database. 
            var phototToReturn = await _photoBusinessLogic.Add(photo);

            return CreatedAtRoute("GetById", new {id = user.Id} ,_mapper.Map<VPhoto>(photo));
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {

            var user = await _userBusinessLogic.GetByUserName(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x=>x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            var resultCurrentMain = await _photoBusinessLogic.Update(currentMain);
            var resultPhoto = await _photoBusinessLogic.Update(photo);

            // if(resultCurrentMain || resultPhoto)
            //     return BadRequest("Failed to change main photo");
            return Ok();
        }

        [HttpDelete("delete-photo/photoId")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userBusinessLogic.GetByUserName(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("You can not delete main photo");

            if(photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            await _userBusinessLogic.DeletePhoto(photoId);
            return Ok();
        }
    }
}