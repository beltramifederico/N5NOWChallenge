using Microsoft.EntityFrameworkCore;
using N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions;
using N5NOW.UserPermissions.IntegrationTests.Helpers;
using N5NOW.UserPermissions.IntegrationTests.Shared;
using System.Net;
using Xunit;

namespace N5NOW.UserPermissions.IntegrationTests.Permissions.GetAll
{
    public class GetPermissionsTests : BaseTest
    {
        private const string Url = "api/v1/permission";

        public GetPermissionsTests(ServerHelper<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetPermission_ShouldReturnsAllPermissions()
        {
            //Arrange
            var permissionsFromDB = await _context.Permissions.
                                            AsNoTracking()
                                            .Include(x => x.PermissionType)
                                            .ToListAsync();


            //Act
            var response = await GetAsync($"{Url}");
            var permissionsResult = GenericDeserializerExtensions
                .DeserializeCustom<List<GetPermissionsResponse>>(response);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
