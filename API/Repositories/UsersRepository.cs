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
    public interface IUsersRepository : IGenericRepositoryBase<AppUser,VMember>
    {
        // public Task<IEnumerable<VMember>> GetAll(IDbConnection connection);
        // public Task<VMember> GetById(IDbConnection connection, int id);
        public Task<VMember> GetByUserName(IDbConnection connection, string username);
        public Task<int> Update(IDbConnection connection, AppUser member);
        public Task<int> DeletePhoto(IDbConnection connection,int photoId);
    }
    public class UsersRepository : GenericRepositoryBase<AppUser,VMember>, IUsersRepository
    {
        public async Task<int> DeletePhoto(IDbConnection connection, int photoId)
        {
            if (connection.State == ConnectionState.Open)
            {
                string sQuery = @"DELETE FROM vPhoto WHERE Id=@Id;";
                var result = await connection.QueryAsync(sQuery, new {Id = photoId});
            }
        
            return 0;
        }

        public override async Task<VMember> GetById(IDbConnection connection, int id)
        {
            if (connection.State == ConnectionState.Open)
            {
                string sQuery = @"SELECT * FROM vMember WHERE Id=@Id;";
                var user = await connection.QueryFirstOrDefaultAsync<VMember>(sQuery, new { Id = id });
            
                sQuery = @"SELECT * FROM vPhoto WHERE AppUserId=@Id;";
                user.Photos = (await connection.QueryAsync<VPhoto>(sQuery, new {Id = id})).ToArray();
            
                return user;
            }
        
            return null;
        }
        
        public async Task<VMember> GetByUserName(IDbConnection connection, string username)
        {
            if (connection.State == ConnectionState.Open)
            {
                string sQuery = @"SELECT * FROM vMember WHERE username=@Username;";
                var user = await connection.QueryFirstOrDefaultAsync<VMember>(sQuery, new {Username = username});

                sQuery = @"SELECT * FROM vPhoto WHERE AppUserId=@Id;";
                user.Photos = (await connection.QueryAsync<VPhoto>(sQuery, new {Id = user.Id})).ToArray();

                return user;
            }

            return null;
        }

        public async Task<int> Update(IDbConnection connection, AppUser user)
        {

            if (connection.State == ConnectionState.Open)
            {
                string sQuery = @"
UPDATE appUser 
SET
userName = @UserName,
dateOfBirth = @dateOfBirth,
knownAs = @knownAs,
gender = @Gender,
introduction = @Introduction,
lookingFor = @LookingFor,
interests = @Interests,
city = @City,
country = @Country 
WHERE Id = @Id;";

                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("userName", user.Username);
                dynamicParameters.Add("dateOfBirth", user.DateOfBirth);
                dynamicParameters.Add("knownAs", user.KnownAs);
                dynamicParameters.Add("gender", user.Gender);
                dynamicParameters.Add("introduction", user.Introduction);
                dynamicParameters.Add("lookingFor", user.LookingFor);
                dynamicParameters.Add("interests", user.Interests);
                dynamicParameters.Add("city", user.City);
                dynamicParameters.Add("country", user.Country);
                dynamicParameters.Add("Id", user.Id);

                return await connection.ExecuteAsync(sQuery, user);
            }

            return 0;
        }
    }
}