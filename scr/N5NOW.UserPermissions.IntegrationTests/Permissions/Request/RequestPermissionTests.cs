using Microsoft.EntityFrameworkCore;
using N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission;
using N5NOW.UserPermissions.IntegrationTests.Helpers;
using N5NOW.UserPermissions.IntegrationTests.Shared;
using System.Net;
using Xunit;

namespace N5NOW.UserPermissions.IntegrationTests.Permissions.Request
{
    public class RequestPermissionTests : BaseTest
    {
        private const string Url = "api/v1/permission/requestpermission";

        public RequestPermissionTests(ServerHelper<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async void RequestPermission_ShouldReturn200OK()
        {
            var permissionsFromDB = await _context.Permissions.
                                            AsNoTracking()
                                            .Include(x => x.PermissionType)
                                            .FirstOrDefaultAsync();

            //Act
            var response = await GetAsync(
                $"{Url}?employeeforename={permissionsFromDB.EmployeeForename}" +
                $"&&employeesurname={permissionsFromDB.EmployeeSurname}");

            var permissionsResult = GenericDeserializerExtensions
                .DeserializeCustom<RequestPermissionResponse>(response);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(permissionsFromDB.Id, permissionsResult.Id);
        }

        [Fact]
        public async void RequestPermission_ShouldReturn500NotFound_IfEmployeeHasNotPermission()
        {
            //Arrange
            var fakeForename = "faketest";
            var fakeSurname = "faketest";

            //Act
            var response = await GetAsync(
                $"{Url}?employeeforename={fakeForename}" +
                $"&&employeesurname={fakeSurname}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
