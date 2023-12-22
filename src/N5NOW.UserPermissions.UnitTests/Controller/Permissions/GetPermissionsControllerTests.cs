using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using N5NOW.UserPermissions.API.Controllers;
using N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions;
using Xunit;

namespace N5NOW.UserPermissions.UnitTests.Controllers.Permissions
{
    public class GetPermissionsControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<PermissionController>> _logger;
        private readonly PermissionController _permissionController;

        public GetPermissionsControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<PermissionController>>();
            _permissionController = new PermissionController(
                _mediator.Object,
                _logger.Object);
        }

        [Fact]
        public async Task GetPermissions_ShouldReturns_200OK()
        {
            //Arrange
            var query = new GetPermissionsResponse();
            var response = new List<GetPermissionsResponse>();

            _mediator.Setup(x => x.Send(It.IsAny<GetPermissionsQuery>(), CancellationToken.None))
                .ReturnsAsync(response);

            //Act
            var result = await _permissionController.GetPermissions() as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
