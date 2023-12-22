using Microsoft.EntityFrameworkCore;
using N5NOW.UserPermissions.Domain.Entities;

namespace N5NOW.UserPermissions.Infrastructure.DAL.Configurations
{
    public class UserPermissionDbContext : DbContext
    {
        public UserPermissionDbContext(DbContextOptions<UserPermissionDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("Users");
        }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<PermissionType> PermissionTypes { get; set; }
    }
}
