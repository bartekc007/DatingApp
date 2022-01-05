using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;

namespace API.Repositories
{
    public class RepositoryBase
    {
        public RepositoryBase(Func<DbConnectionFactory> factory)
        {
            _context = factory;
        }
        protected Func<DbConnectionFactory> _context;
    }
}