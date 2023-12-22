using AutoMapper;
using MediatR;
using N5NOW.UserPermissions.Application.Models.Kafka;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;

namespace N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission
{
    public class RequestPermissionHandler : IRequestHandler<RequestPermissionQuery, RequestPermissionResponse>
    {
        private readonly IKafkaProducer<OperationMessage> _producer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchService<Permission> _elasticSearchService;
        private readonly IMapper _mapper;

        public RequestPermissionHandler(
            IKafkaProducer<OperationMessage> producer,
            IUnitOfWork unitOfWork,
            IElasticSearchService<Permission> elasticSearchService,
            IMapper mapper)
        {
            _producer = producer;
            _unitOfWork = unitOfWork;
            _elasticSearchService = elasticSearchService;
            _mapper = mapper;
        }

        public async Task<RequestPermissionResponse> Handle(RequestPermissionQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.PermissionRepository
                .GetAsync(
                employeeForename: request.EmployeeForename,
                employeeSurname: request.EmployeeSurname);

            if (permission == null)
            {
                throw new Exception("This employee does not has permission");
            }

            await _producer.ProduceAsync("operation-topic",
                new OperationMessage
                {
                    NameOperation = "RequestPermission"
                });

            await _elasticSearchService.AddOrUpdate(permission);

            return _mapper.Map<RequestPermissionResponse>(permission);
        }
    }
}
