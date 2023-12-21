using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using N5NOW.UserPermissions.API.Controllers;
using N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission;
using Xunit;

namespace N5NOW.UserPermissions.UnitTests.Controller.Permissions
{
    public class RequestPermissionTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<PermissionController>> _logger;
        private readonly PermissionController _permissionController;

        public RequestPermissionTests()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<PermissionController>>();
            _permissionController = new PermissionController(
                _mediator.Object,
                _logger.Object);
        }

        [Fact]
        public async Task RequestPermission_ShouldReturns_200OK()
        {
            //Arrange
            var query = new RequestPermissionQuery
            {
                EmployeeForename = "Test",
                EmployeeSurname = "Test"
            };

            var response = new RequestPermissionResponse
            {
                Id = 1,
                EmployeeSurname = "Test",
                EmployeeForename = "Test",
                PermissionDate = DateTime.Now,
                PermissionType = new RequestPermissionTypeResponse
                {
                    Description = "Test",
                }
            };

            _mediator.Setup(x => x.Send(It.IsAny<RequestPermissionQuery>(), CancellationToken.None))
                .ReturnsAsync(response);

            //Act
            var result = await _permissionController.RequestPermission(query) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
