using AutoMapper;
using MediatR;
using N5NOW.UserPermissions.Application.Models.Kafka;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;

namespace N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<GetPermissionsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKafkaProducer<OperationMessage> _producer;
        private readonly IElasticSearchService<Permission> _elasticSearchService;
        public GetPermissionsHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IKafkaProducer<OperationMessage> producer,
            IElasticSearchService<Permission> elasticSearchService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _producer = producer;
            _elasticSearchService = elasticSearchService;
        }

        public async Task<IEnumerable<GetPermissionsResponse>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAllAsync();

            await _producer.ProduceAsync("operation-topic",
                new OperationMessage
                {
                    NameOperation = "GetPermissions"
                });

            await _elasticSearchService.AddOrUpdateBulk(permissions);

            return _mapper.Map<IEnumerable<GetPermissionsResponse>>(permissions);
        }
    }
}
