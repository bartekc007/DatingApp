using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userReposiotry;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userReposiotry, IMapper mapper)
        {
            _mapper = mapper;
            _userReposiotry = userReposiotry;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userReposiotry.GetMembersAsync();
            return Ok(users);
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userReposiotry.GetMemberAsync(username);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userReposiotry.GetUserByUserNameAsync(username);

            _mapper.Map(memberUpdateDto,user);

            _userReposiotry.Update(user);

            if(await _userReposiotry.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");        
        }

    }
}