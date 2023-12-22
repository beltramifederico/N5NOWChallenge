using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5NOW.UserPermissions.Application.Permissions.Commands.Update;
using N5NOW.UserPermissions.Application.Permissions.Queries.GetPermissions;
using N5NOW.UserPermissions.Application.Permissions.Queries.RequestPermission;

namespace N5NOW.UserPermissions.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PermissionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(
            IMediator mediator,
            ILogger<PermissionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("RequestPermission")]
        public async Task<IActionResult> RequestPermission(RequestPermissionQuery query)
        {
            _logger.LogInformation("Invoke RequestPermission() method");
            return Ok(await _mediator.Send(query));
        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> ModifyPermission(ModifyPermissionCommand command)
        {
            _logger.LogInformation("Invoke ModifyPermission() method");
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            _logger.LogInformation("Invoke GetPermissions() method");
            return Ok(await _mediator.Send(new GetPermissionsQuery()));
        }
    }
}
