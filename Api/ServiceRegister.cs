using Api.Domain.Annotations;
using Api.Domain.Secutiry;
using Api.Domain.Users;
using Api.Infra.Database;
using Api.Repositories.Annotations;
using Api.Repositories.Users;
using Api.Services.Annotations;
using Api.Services.Security;
using Api.Services.Users;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class ServiceRegister
    {
        internal static void Register(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddTransient<IAnnotationService, AnnotationService>();
            serviceDescriptors.AddTransient<IAnnotationRepository, AnnotationRepository>();

            serviceDescriptors.AddTransient<IAuthenticationService, AuthenticationService>();

            serviceDescriptors.AddTransient<IUserService, UserService>();
            serviceDescriptors.AddTransient<IUserRepository, UserRepository>();

            RegisterDbContext(serviceDescriptors);
            RegisterMigrationRunner(serviceDescriptors);
        }

        internal static void RegisterDbContext(IServiceCollection serviceDescriptors)
        {
            var dbType = DatabaseConnection.DatabaseType;
            var connectionString = DatabaseConnection.GetConnectionString();
            switch (dbType)
            {
                case DbType.SqlServer:
                    serviceDescriptors.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
                    break;

                case DbType.SAPHana:
                    throw new NotImplementedException();

                case DbType.Postgres:
                    serviceDescriptors.AddEntityFrameworkNpgsql().AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
                    break;
            }
        }

        internal static void RegisterMigrationRunner(IServiceCollection serviceDescriptors)
        {
            var dbType = DatabaseConnection.DatabaseType;
            var connectionString = DatabaseConnection.GetConnectionString();
            serviceDescriptors.AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddFluentMigratorCore();

            switch (dbType)
            {
                case DbType.SqlServer:
                    serviceDescriptors.ConfigureRunner(b => b.AddSqlServer2012()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Program).Assembly).For.Migrations());
                    break;

                case DbType.SAPHana:
                    serviceDescriptors.ConfigureRunner(b => b.AddHana()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Program).Assembly).For.Migrations());
                    break;

                case DbType.Postgres:
                    serviceDescriptors.ConfigureRunner(b => b.AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Program).Assembly).For.Migrations());
                    break;
            }
        }
    }
}
