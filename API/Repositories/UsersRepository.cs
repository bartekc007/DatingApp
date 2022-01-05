using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Dapper;

namespace API.Repositories
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<AppUser>> GetAll();
        public Task<AppUser> GetById(int id);
    }
    public class UsersRepository : RepositoryBase, IUsersRepository
    {
        public UsersRepository(Func<DbConnectionFactory> factory) :base(factory) {}

        public async Task<IEnumerable<AppUser>> GetAll()
        {
             using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                string sQuery = @"SELECT * FROM users;";
                return await connection.QueryAsync<AppUser>(sQuery);
            }
        }

        public async Task<AppUser> GetById(int id)
        {
            using(IDbConnection connection = _context().Connection)
            {
                connection.Open();
                string sQuery = @"SELECT * FROM users WHERE Id=@Id;";
                return await connection.QueryFirstOrDefaultAsync<AppUser>(sQuery, new { Id = id });
            }
        }
    }
}