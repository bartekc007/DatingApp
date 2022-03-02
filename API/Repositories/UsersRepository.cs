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
    }
}