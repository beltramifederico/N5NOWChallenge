using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using N5NOW.UserPermissions.Application.Models.Kafka;
using N5NOW.UserPermissions.Application.Permissions.Commands.Update;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;
using Xunit;

namespace N5NOW.UserPermissions.UnitTests.Handler.Permissions.Commands
{
    public class ModifyPermissionHandlerTests
    {
        private readonly ModifyPermissionHandler _modifyPermissionHandler;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IKafkaProducer<OperationMessage>> _kafkaProducer;
        private readonly Mock<IElasticSearchService<Permission>> _elasticSearchService;
        private readonly ILogger<ModifyPermissionHandler> _logger;

        public ModifyPermissionHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _kafkaProducer = new Mock<IKafkaProducer<OperationMessage>>();
            _elasticSearchService = new Mock<IElasticSearchService<Permission>>();
            _modifyPermissionHandler =
                new ModifyPermissionHandler(
                    _unitOfWorkMock.Object,
                    _logger,
                    _kafkaProducer.Object,
                    _elasticSearchService.Object
                    );
        }

        [Fact]
        public async void ModifyPermission_ShouldReturnUnitValue_IfNotHasException()
        {
            //Arrange 
            var permission = new Permission
            {
                Id = 1,
                EmployeeSurname = "Test",
                EmployeeForename = "Test",
                PermissionDate = DateTime.Now,
                PermissionType = new PermissionType
                {
                    Description = "Test",
                    Id = 1
                }
            };

            var permisionType = new PermissionType
            {
                Description = "test",
                Id = 1
            };

            _unitOfWorkMock.Setup(x =>
                x.PermissionRepository.GetAsync(It.IsAny<int>(), null, null))
                .ReturnsAsync(permission);

            _unitOfWorkMock.Setup(x =>
                x.PermissionTypeRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(permisionType);

            _kafkaProducer.Setup(x =>
                x.ProduceAsync(It.IsAny<string>(), It.IsAny<object>()));

            _elasticSearchService.Setup(x =>
                x.AddOrUpdate(It.IsAny<Permission>()));

            var command = new ModifyPermissionCommand
            {
                Id = 1,
                Permission = new ModifyPermissionCommandDto
                {
                    EmployeeSurname = "Test",
                    EmployeeForename = "Test",
                    PermissionTypeId = 1
                }
            };

            //Act
            var response = await _modifyPermissionHandler
                .Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(response, Unit.Value);
            _unitOfWorkMock.Verify(x =>
                x.PermissionRepository.GetAsync(It.IsAny<int>(), null, null), Times.Once);
            _unitOfWorkMock.Verify(x =>
                x.PermissionTypeRepository.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _kafkaProducer.Verify(x =>
                x.ProduceAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            _elasticSearchService.Verify(x =>
                x.AddOrUpdate(It.IsAny<Permission>()), Times.Once);
        }

        [Fact]
        public void ModifyPermission_ShouldThrowException_IfPermissionNotExists()
        {
            //Arrange 
            Permission permission = null;

            _unitOfWorkMock.Setup(x =>
                x.PermissionRepository.GetAsync(It.IsAny<int>(), null, null))
                .ReturnsAsync(permission);

            var command = new ModifyPermissionCommand
            {
                Id = 1,
                Permission = new ModifyPermissionCommandDto
                {
                    EmployeeSurname = "Test",
                    EmployeeForename = "Test",
                    PermissionTypeId = 1
                }
            };

            //Act
            Func<Task> func = () => _modifyPermissionHandler
                .Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<Exception>(func);
        }

        [Fact]
        public void ModifyPermission_ShouldThrowException_IfPermissionTypeNotExists()
        {
            //Arrange 
            var permission = new Permission
            {
                Id = 1,
                EmployeeSurname = "Test",
                EmployeeForename = "Test",
                PermissionDate = DateTime.Now,
                PermissionType = new PermissionType
                {
                    Description = "Test",
                    Id = 1
                }
            };

            PermissionType permisionType = null;

            _unitOfWorkMock.Setup(x =>
                x.PermissionRepository.GetAsync(It.IsAny<int>(), null, null))
                .ReturnsAsync(permission);

            _unitOfWorkMock.Setup(x =>
                x.PermissionTypeRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(permisionType);

            var command = new ModifyPermissionCommand
            {
                Id = 1,
                Permission = new ModifyPermissionCommandDto
                {
                    EmployeeSurname = "Test",
                    EmployeeForename = "Test",
                    PermissionTypeId = 1
                }
            };

            //Act
            Func<Task> func = () => _modifyPermissionHandler
                .Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<Exception>(func);
        }
    }
}
