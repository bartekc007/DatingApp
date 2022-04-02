using System.Data;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Repositories;

namespace API.BusinessLogics;

public interface IUserBusinessLogic : IGenericBusinessLogicBase<AppUser, VMember>
{
    public Task<VMember> GetByUserName(string username);
    public Task<int> Update(AppUser member);
    public Task<int> DeletePhoto(int photoId);
}

public class UsersBusinessLogic : GenericBusinessLogicBase<AppUser,VMember,IUsersRepository>, IUserBusinessLogic
{
    public UsersBusinessLogic(Func<DbConnectionFactory> factory, IUsersRepository repository) : base(factory,repository) { }

    public async Task<int> DeletePhoto(int photoId)
    {
        using(IDbConnection connection = _context().Connection)
        {
            connection.Open();
            return await _repository.DeletePhoto(connection, photoId);
        }
    }

    public async Task<VMember> GetByUserName(string username)
    {
        using (IDbConnection connection = _context().Connection)
        {
            connection.Open();
            return await _repository.GetByUserName(connection, username);
        }
    }

    public async Task<int> Update(AppUser member)
    {
        using (IDbConnection connection = _context().Connection)
        {
            connection.Open();
            return await _repository.Update(connection, member);
        }
    }
}