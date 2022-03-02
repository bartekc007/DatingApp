using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Services;
using Dapper;

namespace API.Repositories
{
    public interface IAccountRepository
    {
        Task<UserDto> Register(RegisterDto _user);
        Task<UserDto> LogIn(LoginDto _user);
    }
    public class AccountRepository: RepositoryBase, IAccountRepository
    {
        public AccountRepository(Func<DbConnectionFactory> factory, ITokenService tokenService): base (factory) 
        {
            this._tokenService = tokenService;
        }
        private readonly ITokenService _tokenService;

        public async Task<UserDto> LogIn(LoginDto _user)
        {
            using(IDbConnection connection = _context().Connection)
            {
                var sQuery = @"Select * From appuser where username = @username";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("userName",_user.UserName);
                var result = await connection.QuerySingleOrDefaultAsync<AppUser>(sQuery,dynamicParameters);
                if(result == null)
                    return null;

                using var hmac = new HMACSHA512(result.PasswordSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_user.Password));

                for(int i = 0; i< computedHash.Length; i++)
                {
                    if(computedHash[i] != result.PasswordHash[i])
                        result.Username = string.Empty;
                }
                return new UserDto
                {
                    Username = result.Username,
                    Token = _tokenService.CreateToken(result)
                };
            }
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName))
            {
                return null;
            }
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                DateOfBirth = registerDto.DateOfBirth,
                KnownAs = registerDto.KnownAs
            };

            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                string sQuery = @"Insert Into appuser (userName, passwordHash, passwordSalt, dateOfBirth, knownAs) values (@username, @passwordHash, @passwordSalt, @dateOfBirth, @knownAs)";
                connection.Execute(sQuery,user);

                sQuery = @"Select * From appuser where username = @username";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("userName",user.Username);
                var result = await connection.QuerySingleOrDefaultAsync<AppUser>(sQuery,dynamicParameters);
                return new UserDto
                {
                    Username = result.Username,
                    Token = _tokenService.CreateToken(result)
                };
            }
        }

        private async Task<bool> UserExists(string username)
        {
            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("userName",username);
                
                string sQuery = $@"Select * from appUser where username = @username";
                var user = await connection.QueryAsync(sQuery,dynamicParameters);
                return user.Count() >0;
            }
        }
    }
}