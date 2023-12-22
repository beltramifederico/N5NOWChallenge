using Microsoft.EntityFrameworkCore;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.DAL.Configurations;
using N5NOW.UserPermissions.Infrastructure.DAL.Repositories.Interfaces;

namespace N5NOW.UserPermissions.Infrastructure.DAL.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly UserPermissionDbContext _DbContext;

        public PermissionTypeRepository(
            UserPermissionDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<PermissionType> GetByIdAsync(int id)
        {
            return await _DbContext
                .PermissionTypes
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
