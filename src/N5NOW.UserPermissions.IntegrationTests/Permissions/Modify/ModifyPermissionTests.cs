using Microsoft.EntityFrameworkCore;
using N5NOW.UserPermissions.Application.Permissions.Commands.Update;
using N5NOW.UserPermissions.IntegrationTests.Helpers;
using N5NOW.UserPermissions.IntegrationTests.Shared;
using System.Net;
using Xunit;

namespace N5NOW.UserPermissions.IntegrationTests.Permissions.Modify
{
    public class ModifyPermissionTests : BaseTest
    {
        private const string Url = "api/v1/permission";
        public ModifyPermissionTests(ServerHelper<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ModifyPermission_ShouldReturnsOK()
        {
            //Arrange
            var permissionToUpdate = await _context.Permissions.
                                            AsNoTracking()
                                            .Include(x => x.PermissionType)
                                            .FirstOrDefaultAsync(x => x.PermissionTypeId == 1);
            var dataToUpdate = new ModifyPermissionCommand
            {
                Id = permissionToUpdate.Id,
                Permission = new ModifyPermissionCommandDto
                {
                    EmployeeSurname = permissionToUpdate.EmployeeSurname + "Test",
                    EmployeeForename = permissionToUpdate.EmployeeForename + "Test",
                    PermissionTypeId = permissionToUpdate.PermissionTypeId + 1,
                }
            };

            //Act
            var response = await PutAsync($"{Url}/{dataToUpdate?.Id}", dataToUpdate);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModifyPermission_ShouldReturns500_IfPermissionNotExists()
        {
            //Arrange
            var dataToUpdate = new ModifyPermissionCommand
            {
                Id = 1000, // id not exists
                Permission = new ModifyPermissionCommandDto()
            };

            //Act
            var response = await PutAsync($"{Url}/{dataToUpdate?.Id}", dataToUpdate);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
