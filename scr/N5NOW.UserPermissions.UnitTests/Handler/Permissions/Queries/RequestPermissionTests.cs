using AutoMapper;
using Moq;
using N5NOW.UserPermissions.Application.Mapper;
using N5NOW.UserPermissions.Application.Models.Kafka;
using N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;
using Xunit;

namespace N5NOW.UserPermissions.UnitTests.Handler.Permissions.Queries
{
    public class RequestPermissionTests
    {
        private readonly RequestPermissionHandler _getPermissionsHandler;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<IKafkaProducer<OperationMessage>> _kafkaProducer;
        private readonly Mock<IElasticSearchService<Permission>> _elasticSearchService;

        public RequestPermissionTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _unitOfWork = new Mock<IUnitOfWork>();
            _kafkaProducer = new Mock<IKafkaProducer<OperationMessage>>();
            _elasticSearchService = new Mock<IElasticSearchService<Permission>>();

            _getPermissionsHandler =
                new RequestPermissionHandler(
                    _kafkaProducer.Object,
                    _unitOfWork.Object,
                    _elasticSearchService.Object,
                    _mapper);
        }

        [Fact]
        public async void RequestPermissions_ShouldReturnPermission_IfExists()
        {
            //Arrange
            var query = new RequestPermissionQuery
            {
                EmployeeSurname = "Test",
                EmployeeForename = "Test"
            };

            var permission = new Permission
            {
                Id = 1,
                PermissionDate = DateTime.Now,
                EmployeeSurname = query.EmployeeSurname,
                EmployeeForename = query.EmployeeForename,
                PermissionType = new PermissionType
                {
                    Id = 1,
                    Description = "Test",
                }
            };

            _unitOfWork.Setup(x =>
                x.PermissionRepository.GetAsync(null,
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(permission);

            _kafkaProducer.Setup(x =>
               x.ProduceAsync(It.IsAny<string>(), It.IsAny<object>()));

            _elasticSearchService.Setup(x =>
                x.AddOrUpdate(It.IsAny<Permission>()));

            //Act
            var response = await _getPermissionsHandler
                .Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(query.EmployeeSurname, response.EmployeeSurname);
            Assert.Equal(query.EmployeeForename, response.EmployeeForename);
        }

        [Fact]
        public void RequestPermissions_ShouldThrowException_IfNotExists()
        {
            //Arrange
            var query = new RequestPermissionQuery
            {
                EmployeeSurname = "Test",
                EmployeeForename = "Test"
            };

            Permission permission = null;

            _unitOfWork.Setup(x =>
                x.PermissionRepository.GetAsync(null,
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync(permission);

            //Act
            var func = () => _getPermissionsHandler
                .Handle(query, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<Exception>(func);

        }
    }
}
