using Microsoft.EntityFrameworkCore;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.DAL.Configurations;
using N5NOW.UserPermissions.Infrastructure.DAL.Repositories.Interfaces;

namespace N5NOW.UserPermissions.Infrastructure.DAL.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly UserPermissionDbContext _DbContext;

        public PermissionRepository(
            UserPermissionDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _DbContext
                .Permissions
                .Include(x => x.PermissionType)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Permission> GetAsync(int? id = null, string? employeeForename = null, string? employeeSurname = null)
        {
            var query = _DbContext
                .Permissions
                .Include(x => x.PermissionType);

            if (id != null)
            {
                query.Where(x => x.Id == id);
            }

            if (employeeForename != null)
            {
                query.Where(x => x.EmployeeForename == employeeForename);
            }

            if (employeeSurname != null)
            {
                query.Where(x => x.EmployeeSurname == employeeSurname);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            if (await GetAsync(permission.Id) == null)
            {
                throw new Exception();
            }

            _DbContext.Update(permission);
        }
    }
}
