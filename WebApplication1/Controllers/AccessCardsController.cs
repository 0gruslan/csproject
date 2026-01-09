using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models.DTO;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/access-cards")]
[Authorize]
[Produces("application/json")]
public class AccessCardsController : ControllerBase
{
    private readonly IAccessCardService _accessCardService;
    private readonly ILogger<AccessCardsController> _logger;

    public AccessCardsController(IAccessCardService accessCardService, ILogger<AccessCardsController> logger)
    {
        _accessCardService = accessCardService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<AccessCardDto>>> GetAll()
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var result = await _accessCardService.GetAllAsync(currentUserId, currentUserRoles);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AccessCardDto>> GetById(int id)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var accessCard = await _accessCardService.GetByIdAsync(id, currentUserId, currentUserRoles);
        if (accessCard == null)
            return NotFound();

        return Ok(accessCard);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AccessCardDto>> Create([FromBody] CreateAccessCardDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var accessCard = await _accessCardService.CreateAsync(dto, currentUserId, currentUserRoles);
        _logger.LogInformation("Access card created: {AccessCardId} by {CurrentUserId}", accessCard.Id, currentUserId);
        return CreatedAtAction(nameof(GetById), new { id = accessCard.Id }, accessCard);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AccessCardDto>> Update(int id, [FromBody] UpdateAccessCardDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRoles = GetCurrentUserRoles();

        var accessCard = await _accessCardService.UpdateAsync(id, dto, currentUserId, currentUserRoles);
        _logger.LogInformation("Access card updated: {AccessCardId} by {CurrentUserId}", id, currentUserId);
        return Ok(accessCard);
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

        await _accessCardService.DeleteAsync(id, currentUserId, currentUserRoles);
        _logger.LogInformation("Access card deleted: {AccessCardId} by {CurrentUserId}", id, currentUserId);
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


