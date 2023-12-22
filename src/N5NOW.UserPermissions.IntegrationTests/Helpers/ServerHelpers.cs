using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.DAL.Configurations;

namespace N5NOW.UserPermissions.IntegrationTests.Helpers
{
    public class ServerHelper<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        public void Seeding(UserPermissionDbContext context)
        {
            if (!context.Permissions.Any())
            {
                context.Permissions.AddRange(
                    new List<Permission>
                    {
                        new ()
                        {
                            EmployeeSurname = "Test",
                            EmployeeForename = "Test",
                            Id = 1,
                            PermissionDate = DateTime.UtcNow,
                            PermissionTypeId = 1
                        },
                        new ()
                        {
                            EmployeeSurname = "Test2",
                            EmployeeForename = "Test2",
                            Id = 2,
                            PermissionDate = DateTime.UtcNow,
                            PermissionTypeId = 2
                        }
                    });
            }

            if (!context.PermissionTypes.Any())
            {
                context.PermissionTypes.AddRange(
                    new List<PermissionType>
                    {
                        new ()
                        {
                            Id = 1,
                            Description = "Test"
                        },
                        new ()
                        {
                            Id = 2,
                            Description = "Test2"
                        },
                    });
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<UserPermissionDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<UserPermissionDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DbInMemory");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<UserPermissionDbContext>())
                {
                    appContext.Database.EnsureCreated();
                    Seeding(appContext);
                    appContext.SaveChanges();
                }
            });
        }
    }
}
