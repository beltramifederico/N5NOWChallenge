namespace N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions
{
    public class GetPermissionsResponse
    {
        public int Id { get; set; }

        public string EmployeeForename { get; set; }

        public string EmployeeSurname { get; set; }

        public int PermissionTypeId { get; set; }

        public PermissionTypeResponse PermissionType { get; set; }

        public DateTime PermissionDate { get; set; }
    }
    public class PermissionTypeResponse
    {
        public string Description { get; set; }
    }
}
