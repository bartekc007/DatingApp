using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public UsersController(IUsersRepository _users, IMapper _mapper)
        {
            this._mapper = _mapper;
            _usersRepository = _users;
        }
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
        {
            var result = await _usersRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetById(int id)
        {
            var result = await _usersRepository.GetById(id);
            return result;
        }

        [HttpPut]
        public async Task<ActionResult> Update(MemberDto member)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                var user = await _usersRepository.GetByUserName(username);
                _mapper.Map(member, user);
                if(await _usersRepository.Update(member)==1)
                    return NoContent();
                return BadRequest();
            }
            return BadRequest();
        }
    }
}