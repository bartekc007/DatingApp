using System.Data;
using API.DTOs;
using API.Entities;
using Dapper;

namespace API.Repositories;

public interface IGenericRepositoryBase<TTable, TView> where TTable : EntityBase
                                                       where TView : DtoBase
{
    public Task<IEnumerable<TView>> GetAll(IDbConnection connection);
    public Task<TView> GetById(IDbConnection connection, int id);
}

public class GenericRepositoryBase<TTable,TView> : IGenericRepositoryBase<TTable,TView> where TTable : EntityBase
                                                 where TView : DtoBase
{
    private readonly string _table;
    private readonly string _view;

    public GenericRepositoryBase()
    {
        this._table = typeof(TTable).ToString().Substring(typeof(TTable).ToString().LastIndexOf('.')+1);
        this._view = typeof(TView).ToString().Substring(typeof(TView).ToString().LastIndexOf('.')+1);
    }

    public virtual async Task<IEnumerable<TView>> GetAll(IDbConnection connection)
    {
        if (connection.State == ConnectionState.Open)
        {
            string sQuery = $"SELECT * FROM {_view};";
            var entities = await connection.QueryAsync<TView>(sQuery);
            return entities;
        }

        return null;
    }

    public virtual async Task<TView> GetById(IDbConnection connection, int id)
    {
        if (connection.State == ConnectionState.Open)
        {
            string sQuery = @$"SELECT * FROM {_view} WHERE Id=@Id;";
            var entity = await connection.QueryFirstOrDefaultAsync<TView>(sQuery, new { Id = id });
            return entity;
        }

        return null;
    }
}