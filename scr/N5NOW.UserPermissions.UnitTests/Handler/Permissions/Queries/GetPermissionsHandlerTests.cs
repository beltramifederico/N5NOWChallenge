using AutoMapper;
using Moq;
using N5NOW.UserPermissions.Application.Mapper;
using N5NOW.UserPermissions.Application.Models.Kafka;
using N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;
using Xunit;

namespace N5NOW.UserPermissions.UnitTests.Handler.Permissions.Queries
{
    public class GetPermissionsHandlerTests
    {
        private readonly GetPermissionsHandler _getPermissionsHandler;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<IKafkaProducer<OperationMessage>> _kafkaProducer;
        private readonly Mock<IElasticSearchService<Permission>> _elasticSearchService;

        public GetPermissionsHandlerTests()
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
                new GetPermissionsHandler(
                    _unitOfWork.Object,
                    _mapper,
                    _kafkaProducer.Object,
                    _elasticSearchService.Object);
        }

        [Fact]
        public async void GetPermissions_ShouldReturnAllPermissions()
        {
            //Arrange
            var query = new GetPermissionsQuery();

            var permissions = new List<Permission> {
                new Permission{
                    Id = 1,
                    PermissionDate = DateTime.Now,
                    EmployeeSurname = "Test",
                    EmployeeForename = "Test",
                    PermissionType = new PermissionType
                    {
                        Description = "Test",
                    }
                },
                    new Permission{
                    Id = 1,
                    PermissionDate = DateTime.Now,
                    EmployeeSurname = "Test",
                    EmployeeForename = "Test",
                    PermissionType = new PermissionType
                    {
                        Description = "Test",
                    }
                }
            };

            _unitOfWork.Setup(x =>
                x.PermissionRepository.GetAllAsync())
                .ReturnsAsync(permissions);

            _kafkaProducer.Setup(x =>
               x.ProduceAsync(It.IsAny<string>(), It.IsAny<object>()));

            _elasticSearchService.Setup(x =>
                x.AddOrUpdate(It.IsAny<Permission>()));

            //Act
            var response = await _getPermissionsHandler
                .Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(response.Count(), permissions.Count);
        }


    }
}
