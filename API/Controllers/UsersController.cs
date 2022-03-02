using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public UsersController(IUsersRepository _users)
        {
            _usersRepository = _users;
        }
        private readonly IUsersRepository _usersRepository;

        [HttpGet]
        public async Task<IEnumerable<MemberDto>> GetAll()
        {
            var result = await _usersRepository.GetAll();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<MemberDto> GetById(int id)
        {
            var result = await _usersRepository.GetById(id);
            return result;
        }
    }
}