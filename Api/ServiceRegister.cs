using Api.Domain;
using Api.Domain.Annotations;
using Api.Domain.ComplexEntityAndAggregates;
using Api.Domain.Secutiry;
using Api.Domain.Tests;
using Api.Domain.UserAuthorizations;
using Api.Domain.Users;
using Api.Infra.Database;
using Api.Infra.Security;
using Api.Repositories.Annotations;
using Api.Repositories.ComplexEntityAndAggregates;
using Api.Repositories.Tests;
using Api.Repositories.UserAuthorizations;
using Api.Repositories.Users;
using Api.Services;
using Api.Services.Annotations;
using Api.Services.ComplexEntityAndAggregates;
using Api.Services.Security;
using Api.Services.Tests;
using Api.Services.UserAuthorizations;
using Api.Services.Users;
using FluentMigrator.Runner;
using Google.Authenticator;
using LinqToDB;
using Microsoft.EntityFrameworkCore;
using Shyjus.BrowserDetection;

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

            serviceDescriptors.AddHttpContextAccessor();
            //serviceDescriptors.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            serviceDescriptors.AddTransient<IClaimsRepository, ClaimsRepository>();

            serviceDescriptors.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
            serviceDescriptors.AddTransient<IUserAuthorizationRepository, UserAuthorizationRepository>();

            serviceDescriptors.AddTransient<ISharedService, SharedService>();

            serviceDescriptors.AddTransient<ITestService, TestService>();
            serviceDescriptors.AddTransient<ITestRepository, TestRepository>();

            serviceDescriptors.AddTransient<TwoFactorAuthenticator, TwoFactorAuthenticator>();

            serviceDescriptors.AddTransient<IComplexPrincipalService, ComplexPrincipalService>();
            serviceDescriptors.AddTransient<IComplexPrincipalRepository, ComplexPrincipalRepository>();

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
                    serviceDescriptors.AddDbContext<DataContextEF>(options => 
                        options.UseSqlServer(connectionString)
                            .EnableSensitiveDataLogging());
                    serviceDescriptors.AddTransient(s =>
                        new DataContextLQ(ProviderName.SqlServer2022, connectionString));
                    break;

                case DbType.SAPHana:
                    serviceDescriptors.AddTransient(s =>
                        new DataContextLQ(ProviderName.SapHana, connectionString));
                    throw new NotImplementedException();

                case DbType.Postgres:
                    serviceDescriptors/*.AddEntityFrameworkNpgsql()*/
                        .AddDbContext<DataContextEF>(options => 
                            options.UseNpgsql(connectionString)
                                .EnableSensitiveDataLogging());
                    serviceDescriptors.AddTransient(s => 
                        new DataContextLQ(ProviderName.PostgreSQL15, connectionString));
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
