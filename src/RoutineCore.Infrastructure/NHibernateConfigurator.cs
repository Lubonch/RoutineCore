using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using RoutineCore.Infrastructure.Mappings;

namespace RoutineCore.Infrastructure
{
    public static class NHibernateConfigurator
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            var sessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EmployeeMap>())
                .ExposeConfiguration(cfg =>
                {
                    // Create schemas automatically if they don't exist (useful for a simplified replica)
                    var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg);
                    schemaExport.Execute(false, true);
                })
                .BuildSessionFactory();

            services.AddSingleton<ISessionFactory>(sessionFactory);
            services.AddScoped<ISession>(provider => provider.GetRequiredService<ISessionFactory>().OpenSession());

            return services;
        }
    }
}
