using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace N5NOW.UserPermissions.Application.Permissions.Commands.Update
{
    public class ModifyPermissionCommand : IRequest<Unit>
    {
        [FromRoute]
        public int Id { get; set; }

        [FromBody]
        public ModifyPermissionCommandDto Permission { get; set; }
    }

    public class ModifyPermissionCommandDto
    {
        public string EmployeeForename { get; set; }

        public string EmployeeSurname { get; set; }

        public int PermissionTypeId { get; set; }
    }
}
