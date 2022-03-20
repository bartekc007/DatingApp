using System.Data;
using API.Data;
using API.Entities;
using API.Repositories;

namespace API.BusinessLogics;

public interface IGenericBusinessLogicBase<TTable,TView> where TTable : EntityBase
                                                         where TView : DtoBase
{
    public Task<IEnumerable<TView>> GetAll();
    public Task<TView> GetById(int id);
}

public class GenericBusinessLogicBase<TTable,TView, TRepository> 
    : BusinessLogicBase, IGenericBusinessLogicBase<TTable, TView> where TTable : EntityBase
                                         where TView : DtoBase
                                         where TRepository : IGenericRepositoryBase<TTable,TView>
{
    protected TRepository _repository;
    
    public GenericBusinessLogicBase(Func<DbConnectionFactory> factory, TRepository repository) : base(factory)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TView>> GetAll()
    {
        using (IDbConnection connection = _context().Connection)
        {
            connection.Open();
            return await _repository.GetAll(connection);
        }
    }

    public async Task<TView> GetById(int id)
    {
        using (IDbConnection connection = _context().Connection)
        {
            connection.Open();
            return await _repository.GetById(connection, id);
        }
    }
}