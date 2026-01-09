using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models.DTO;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/access-points")]
[Authorize]
[Produces("application/json")]
public class AccessPointsController : ControllerBase
{
    private readonly IAccessPointService _accessPointService;
    private readonly ILogger<AccessPointsController> _logger;

    public AccessPointsController(IAccessPointService accessPointService, ILogger<AccessPointsController> logger)
    {
        _accessPointService = accessPointService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResponseDto<AccessPointDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var result = await _accessPointService.GetAllAsync(page, pageSize, search, currentUserId, currentUserRoles);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AccessPointDto>> GetById(int id)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var accessPoint = await _accessPointService.GetByIdAsync(id, currentUserId, currentUserRoles);
        if (accessPoint == null)
            return NotFound();

        return Ok(accessPoint);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AccessPointDto>> Create([FromBody] CreateAccessPointDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var accessPoint = await _accessPointService.CreateAsync(dto, currentUserId, currentUserRoles);
        _logger.LogInformation("Access point created: {AccessPointId} by {CurrentUserId}", accessPoint.Id, currentUserId);
        return CreatedAtAction(nameof(GetById), new { id = accessPoint.Id }, accessPoint);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AccessPointDto>> Update(int id, [FromBody] UpdateAccessPointDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var accessPoint = await _accessPointService.UpdateAsync(id, dto, currentUserId, currentUserRoles);
        _logger.LogInformation("Access point updated: {AccessPointId} by {CurrentUserId}", id, currentUserId);
        return Ok(accessPoint);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        await _accessPointService.DeleteAsync(id, currentUserId, currentUserRoles);
        _logger.LogInformation("Access point deleted: {AccessPointId} by {CurrentUserId}", id, currentUserId);
        return NoContent();
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


