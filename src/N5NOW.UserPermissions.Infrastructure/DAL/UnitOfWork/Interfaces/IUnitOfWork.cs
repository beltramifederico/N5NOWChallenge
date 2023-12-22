using N5NOW.UserPermissions.Infrastructure.DAL.Repositories.Interfaces;

namespace N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository PermissionRepository { get; }

        IPermissionTypeRepository PermissionTypeRepository { get; }

        Task Save();
    }
}
