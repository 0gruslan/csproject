using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models.DTO;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/access-logs")]
[Authorize]
[Produces("application/json")]
public class AccessLogsController : ControllerBase
{
    private readonly IAccessLogService _accessLogService;
    private readonly ILogger<AccessLogsController> _logger;

    public AccessLogsController(IAccessLogService accessLogService, ILogger<AccessLogsController> logger)
    {
        _accessLogService = accessLogService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<AccessLogDto>>> GetAll()
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var result = await _accessLogService.GetAllAsync(currentUserId, currentUserRoles);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AccessLogDto>> GetById(int id)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var accessLog = await _accessLogService.GetByIdAsync(id, currentUserId, currentUserRoles);
        if (accessLog == null)
            return NotFound();

        return Ok(accessLog);
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    private List<string> GetCurrentUserRoles()
    {
        return User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
    }
}


