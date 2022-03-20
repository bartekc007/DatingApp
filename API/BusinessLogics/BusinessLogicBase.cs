using API.Data;

namespace API.BusinessLogics;

public class BusinessLogicBase
{
    public BusinessLogicBase(Func<DbConnectionFactory> factory)
    {
        _context = factory;
    }
    protected Func<DbConnectionFactory> _context;
}