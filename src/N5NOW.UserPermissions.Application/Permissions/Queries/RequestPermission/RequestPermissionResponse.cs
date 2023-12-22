namespace N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission
{
    public class RequestPermissionResponse
    {
        public int Id { get; set; }

        public string EmployeeForename { get; set; }

        public string EmployeeSurname { get; set; }

        public int PermissionTypeId { get; set; }

        public RequestPermissionTypeResponse PermissionType { get; set; }

        public DateTime PermissionDate { get; set; }
    }

    public class RequestPermissionTypeResponse
    {
        public string Description { get; set; }
    }
}
