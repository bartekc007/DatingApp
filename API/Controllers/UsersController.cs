using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.BusinessLogics;
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
        public UsersController(IUserBusinessLogic _users, IMapper _mapper)
        {
            this._mapper = _mapper;
            _userBusinessLogic = _users;
        }
        private readonly IUserBusinessLogic _userBusinessLogic;
        private readonly IMapper _mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VMember>>> GetAll()
        {
            var result = await _userBusinessLogic.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VMember>> GetById(int id)
        {
            var result = await _userBusinessLogic.GetById(id);
            return result;
        }

        [HttpPut]
        public async Task<ActionResult> Update(VMember vMember)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                var user = AppUser.CopyFrom(vMember);

                if(await _userBusinessLogic.Update(user)==1)
                    return NoContent();
                return BadRequest();
            }
            return BadRequest();
        }
    }
}