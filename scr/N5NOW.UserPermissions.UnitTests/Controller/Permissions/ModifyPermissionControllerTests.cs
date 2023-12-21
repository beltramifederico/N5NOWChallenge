using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using N5NOW.UserPermissions.API.Controllers;
using N5NOW.UserPermissions.Application.Permissions.Commands.Update;
using Xunit;

namespace N5NOW.UserPermissions.UnitTests.Controllers.Permissions
{
    public class ModifyPermissionControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<PermissionController>> _logger;
        private readonly PermissionController _permissionController;

        public ModifyPermissionControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<PermissionController>>();
            _permissionController = new PermissionController(
                _mediator.Object,
                _logger.Object);
        }

        [Fact]
        public async Task ModifyPermission_ShouldReturns_200OK()
        {
            //Arrange
            var command = new ModifyPermissionCommand
            {
                Id = 1,
                Permission = new ModifyPermissionCommandDto
                {
                    EmployeeSurname = "SurnameTest",
                    EmployeeForename = "ForeNameTest",
                    PermissionTypeId = 1,
                }
            };
            _mediator.Setup(x => x.Send(It.IsAny<ModifyPermissionCommand>(), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            //Act
            var result = await _permissionController.ModifyPermission(command) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
