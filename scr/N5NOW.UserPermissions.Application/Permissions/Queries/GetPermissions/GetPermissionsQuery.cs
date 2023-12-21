using MediatR;

namespace N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions
{
    public class GetPermissionsQuery : IRequest<IEnumerable<GetPermissionsResponse>>
    {
    }
}
