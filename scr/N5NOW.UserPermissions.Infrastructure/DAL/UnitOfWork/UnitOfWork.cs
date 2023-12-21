using N5NOW.UserPermissions.Infrastructure.DAL.Configurations;
using N5NOW.UserPermissions.Infrastructure.DAL.Repositories.Interfaces;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;

namespace N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserPermissionDbContext _context;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionTypeRepository _permissionTypeRepository;

        public UnitOfWork(
            UserPermissionDbContext context,
            IPermissionRepository permissionRepository,
            IPermissionTypeRepository permissionTypeRepository)
        {
            _context = context;
            _permissionRepository = permissionRepository;
            _permissionTypeRepository = permissionTypeRepository;
        }

        public IPermissionRepository PermissionRepository => _permissionRepository;

        public IPermissionTypeRepository PermissionTypeRepository => _permissionTypeRepository;

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
