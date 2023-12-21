using MediatR;
using Microsoft.Extensions.Logging;
using N5NOW.UserPermissions.Application.Models.Kafka;
using N5NOW.UserPermissions.Domain.Entities;
using N5NOW.UserPermissions.Infrastructure.Analitycs.ElasticSearch.Interface;
using N5NOW.UserPermissions.Infrastructure.DAL.UnitOfWork.Interfaces;
using N5NOW.UserPermissions.Infrastructure.Streaming.Kafka.Interface;

namespace N5NOW.UserPermissions.Application.Permissions.Commands.Update
{
    public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ModifyPermissionHandler> _logger;
        private readonly IKafkaProducer<OperationMessage> _producer;
        private readonly IElasticSearchService<Permission> _elasticSearchService;

        public ModifyPermissionHandler(
            IUnitOfWork unitOfWork,
            ILogger<ModifyPermissionHandler> logger,
            IKafkaProducer<OperationMessage> producer,
            IElasticSearchService<Permission> elasticSearchService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _producer = producer;
            _elasticSearchService = elasticSearchService;
        }

        public async Task<Unit> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.PermissionRepository.GetAsync(request.Id);

            if (permission == null)
            {
                throw new Exception("Permission to modify does not exists");
            }

            permission.EmployeeSurname = request.Permission.EmployeeSurname;
            permission.EmployeeForename = request.Permission.EmployeeForename;
            permission.PermissionDate = DateTime.UtcNow;
            var permissionType = await _unitOfWork.PermissionTypeRepository
                .GetByIdAsync(request.Permission.PermissionTypeId);

            if (permissionType == null)
            {
                throw new Exception("PermissionType does not exists");
            }
            permission.PermissionType = permissionType;

            try
            {
                await _unitOfWork.PermissionRepository.UpdateAsync(permission);
            }
            catch (Exception ex)
            {
                _unitOfWork.Dispose();
                _logger.LogError($"Exception trying save changes: {ex.Message}");
            }

            await _unitOfWork.Save();

            await _producer.ProduceAsync("operation-topic",
                new OperationMessage
                {
                    NameOperation = "ModifyPermission"
                });

            await _elasticSearchService.AddOrUpdate(permission);

            return Unit.Value;
        }
    }
}
