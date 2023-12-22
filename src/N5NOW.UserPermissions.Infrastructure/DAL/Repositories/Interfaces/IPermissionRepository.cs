using N5NOW.UserPermissions.Domain.Entities;

namespace N5NOW.UserPermissions.Infrastructure.DAL.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();

        Task<Permission> GetAsync(int? id = null, string? employeeForename = null, string? employeeSurname = null);

        Task UpdateAsync(Permission permission);
    }
}
