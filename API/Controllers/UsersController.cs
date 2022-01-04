using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsersController : ControllerBase
    {
        public UsersController(IUsersRepository _users)
        {
            _usersRepository = _users;
        }
        private readonly IUsersRepository _usersRepository;

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetAll()
        {
            var result = await _usersRepository.GetAll();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<AppUser> GetById(int id)
        {
            var result = await _usersRepository.GetById(id);
            return result;
        }
    }
}