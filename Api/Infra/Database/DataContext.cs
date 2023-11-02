using Api.Domain.Annotations;
using Api.Domain.UserAuthorizations;
using Api.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Metadata;

namespace Api.Infra.Database
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            /*ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.LazyLoadingEnabled = false;*/
        }

        public DataContext(DbContextOptions<DataContext> options)
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

        public DbSet<Annotation> Annotation { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAuthorization> UserAuthorization { get; set; }
    }
}
