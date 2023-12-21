using N5NOW.UserPermissions.Domain.Entities;

namespace N5NOW.UserPermissions.Infrastructure.DAL.Repositories.Interfaces
{
    public interface IPermissionTypeRepository
    {
        Task<PermissionType> GetByIdAsync(int id);
    }
}
