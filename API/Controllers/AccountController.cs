using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public interface IAccountController
    {
        Task<ActionResult<UserDto>> Register(RegisterDto registerDto);
        Task<ActionResult<UserDto>> LogIn(LoginDto loginDto);
    }
    public class AccountController : BaseApiController, IAccountController
    {
        public AccountController(IAccountRepository accountRepository)
        {
            this._accountRepository = accountRepository;
        }
        private readonly IAccountRepository _accountRepository;

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var result = await _accountRepository.Register(registerDto);
            if(result == null)
                return BadRequest("User already exists");
            return result;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LogIn(LoginDto loginDto)
        {
            var result = await _accountRepository.LogIn(loginDto);
            if(result == null)
                return Unauthorized("Invalid username");
            else if(result.Username == string.Empty)
                return Unauthorized("Invalid password");
            return result;
        }
    }
}