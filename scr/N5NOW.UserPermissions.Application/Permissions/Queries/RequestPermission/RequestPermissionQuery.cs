using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission
{
    public class RequestPermissionQuery : IRequest<RequestPermissionResponse>
    {
        [FromQuery]
        public string EmployeeForename { get; set; }

        [FromQuery]
        public string EmployeeSurname { get; set; }
    }
}
