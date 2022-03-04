using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Dapper;

namespace API.Repositories
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<MemberDto>> GetAll();
        public Task<MemberDto> GetById(int id);
        public Task<MemberDto> GetByUserName(string username);
        public Task<int> Update(MemberDto member);
    }
    public class UsersRepository : RepositoryBase, IUsersRepository
    {
        public UsersRepository(Func<DbConnectionFactory> factory) :base(factory) {}

        public async Task<IEnumerable<MemberDto>> GetAll()
        {
             using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                string sQuery = @"SELECT * FROM vMember;";
                return await connection.QueryAsync<MemberDto>(sQuery);
            }
        }

        public async Task<MemberDto> GetById(int id)
        {
            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                string sQuery = @"SELECT * FROM vMember WHERE Id=@Id;";
                var user = await connection.QueryFirstOrDefaultAsync<MemberDto>(sQuery, new { Id = id });
                
                sQuery = @"SELECT * FROM vPhoto WHERE AppUserId=@Id;";
                user.Photos = (await connection.QueryAsync<PhotoDto>(sQuery, new {Id = id})).ToArray();
                
                return user;
            }
        }
        
        public async Task<MemberDto> GetByUserName(string username)
        {
            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                string sQuery = @"SELECT * FROM vMember WHERE username=@Username;";
                var user = await connection.QueryFirstOrDefaultAsync<MemberDto>(sQuery, new { Username = username });
                
                sQuery = @"SELECT * FROM vPhoto WHERE AppUserId=@Id;";
                user.Photos = (await connection.QueryAsync<PhotoDto>(sQuery, new {Id = user.Id})).ToArray();
                
                return user;
            }
        }

        public async Task<int> Update(MemberDto user)
        {
            using (IDbConnection connection = _context().Connection)
            {
                connection.Open();
                string sQuery = $"UPDATE appUser SET" +
                                "userName=@UserName, " +
                                "dateOfBirth=@dateOfBirth, " +
                                "knownAs=@KnownUs, " +
                                "gender=@Gender, " +
                                "introduction=@Introduction, " +
                                "lookingFor=@LookingFor, " +
                                "interests@Interests, " +
                                "city=@City, " +
                                "country=@Country " +
                                "WHERE appUserId=@Id";
                
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("userName",user.Username);
                dynamicParameters.Add("dateOfBirth",user.DateOfBirth);
                dynamicParameters.Add("knownAs",user.KnownAs);
                dynamicParameters.Add("gender",user.Gender);
                dynamicParameters.Add("introduction",user.Introduction);
                dynamicParameters.Add("lookingFor",user.LookingFor);
                dynamicParameters.Add("interests",user.Interests);
                dynamicParameters.Add("city",user.City);
                dynamicParameters.Add("country",user.Country);
                
                return await connection.ExecuteAsync(sQuery,user);
            }
        }
    }
}