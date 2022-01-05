using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Repositories;
using API.Services;
using Autofac;

namespace API.Extensions
{
    public class DIContainer : Module
    {
        private string _connectionString;

        public DIContainer(string connectionString)
        {
            this._connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {  
            // Database context 
            builder.Register(ctx => new DbConnectionFactory(_connectionString))
                .Named<DbConnectionFactory>(nameof(DbConnectionFactory))
                .InstancePerDependency();
            builder.Register<Func<DbConnectionFactory>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return () => cc.ResolveNamed<DbConnectionFactory>(nameof(DbConnectionFactory));
            });

            // Token Service
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();

            // Repositories
            builder.RegisterType<UsersRepository>().As<IUsersRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
        }
    }
}