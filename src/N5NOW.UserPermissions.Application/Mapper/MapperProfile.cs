using AutoMapper;
using N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions;
using N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission;
using N5NOW.UserPermissions.Domain.Entities;

namespace N5NOW.UserPermissions.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Permission, GetPermissionsResponse>()
                .ReverseMap();

            CreateMap<PermissionTypeResponse, PermissionType>()
                .ReverseMap();

            CreateMap<RequestPermissionTypeResponse, PermissionType>()
                .ReverseMap();

            CreateMap<Permission, RequestPermissionResponse>()
                .ReverseMap();
        }
    }
}
