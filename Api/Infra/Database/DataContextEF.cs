using Api.Domain.Annotations;
using Api.Domain.ComplexEntityAndAggregates;
using Api.Domain.Tests;
using Api.Domain.UserAuthorizations;
using Api.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Infra.Database
{
    public class DataContextEF : DbContext
    {
        public DataContextEF()
        {
            /*ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;*/
        }

        public DataContextEF(DbContextOptions<DataContextEF> options)
            : base(options)
        {
            /*ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;*/
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging(); //Mostra detalhes de conflitos

            //base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Test> Test { get; set; }

        public DbSet<Annotation> Annotation { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAuthorization> UserAuthorization { get; set; }

        public DbSet<ComplexPrincipal> ComplexPrincipal { get; set; }
        public DbSet<ComplexAggregate> ComplexAggregate { get; set; }
        public DbSet<ComplexSubAggregate> ComplexSubAggregate { get; set; }
        public DbSet<ComplexSimpleFK> ComplexSimpleFK { get; set; }
    }
}
