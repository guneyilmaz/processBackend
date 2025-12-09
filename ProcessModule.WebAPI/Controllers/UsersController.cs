using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessModule.Application.DTOs;
using ProcessModule.Application.Features.Users.Commands;
using ProcessModule.Application.Features.Users.Queries;

namespace ProcessModule.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // JWT authentication required for all endpoints
public class UsersController : ControllerBase
{
    private readonly GetAllUsersHandler _getAllUsersHandler;
    private readonly GetUserByIdHandler _getUserByIdHandler;
    private readonly GetUserByEmailHandler _getUserByEmailHandler;
    private readonly CreateUserHandler _createUserHandler;
    private readonly UpdateUserHandler _updateUserHandler;
    private readonly DeleteUserHandler _deleteUserHandler;
    private readonly ToggleUserStatusHandler _toggleUserStatusHandler;
    private readonly ConfirmEmailHandler _confirmEmailHandler;
    private readonly CheckUserExistsHandler _checkUserExistsHandler;
    private readonly CheckEmailExistsHandler _checkEmailExistsHandler;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        GetAllUsersHandler getAllUsersHandler,
        GetUserByIdHandler getUserByIdHandler,
        GetUserByEmailHandler getUserByEmailHandler,
        CreateUserHandler createUserHandler,
        UpdateUserHandler updateUserHandler,
        DeleteUserHandler deleteUserHandler,
        ToggleUserStatusHandler toggleUserStatusHandler,
        ConfirmEmailHandler confirmEmailHandler,
        CheckUserExistsHandler checkUserExistsHandler,
        CheckEmailExistsHandler checkEmailExistsHandler,
        ILogger<UsersController> logger)
    {
        _getAllUsersHandler = getAllUsersHandler;
        _getUserByIdHandler = getUserByIdHandler;
        _getUserByEmailHandler = getUserByEmailHandler;
        _createUserHandler = createUserHandler;
        _updateUserHandler = updateUserHandler;
        _deleteUserHandler = deleteUserHandler;
        _toggleUserStatusHandler = toggleUserStatusHandler;
        _confirmEmailHandler = confirmEmailHandler;
        _checkUserExistsHandler = checkUserExistsHandler;
        _checkEmailExistsHandler = checkEmailExistsHandler;
        _logger = logger;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await _getAllUsersHandler.Handle(query);

        if (!result.Success)
        {
            _logger.LogError("Error getting all users: {ErrorMessage}", result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        return Ok(result.Users);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await _getUserByIdHandler.Handle(query);

        if (!result.Success)
        {
            _logger.LogError("Error getting user with ID {UserId}: {ErrorMessage}", id, result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        if (result.User == null)
            return NotFound($"User with ID {id} not found");

        return Ok(result.User);
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
    {
        var query = new GetUserByEmailQuery { Email = email };
        var result = await _getUserByEmailHandler.Handle(query);

        if (!result.Success)
        {
            _logger.LogError("Error getting user with email {Email}: {ErrorMessage}", email, result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        if (result.User == null)
            return NotFound($"User with email {email} not found");

        return Ok(result.User);
    }

    /// <summary>
    /// Create new user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new CreateUserCommand { CreateUserDto = createUserDto };
        var result = await _createUserHandler.Handle(command);

        if (!result.Success)
        {
            _logger.LogError("Error creating user {Email}: {ErrorMessage}", createUserDto.Email, result.ErrorMessage);
            
            if (result.ErrorMessage?.Contains("already exists") == true)
                return BadRequest(result.ErrorMessage);
            
            return StatusCode(500, result.ErrorMessage);
        }

        _logger.LogInformation("User created successfully: {Email}", createUserDto.Email);
        return CreatedAtAction(nameof(GetUser), new { id = result.User!.Id }, result.User);
    }

    /// <summary>
    /// Update user
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new UpdateUserCommand { Id = id, UpdateUserDto = updateUserDto };
        var result = await _updateUserHandler.Handle(command);

        if (!result.Success)
        {
            _logger.LogError("Error updating user with ID {UserId}: {ErrorMessage}", id, result.ErrorMessage);
            
            if (result.ErrorMessage?.Contains("not found") == true)
                return NotFound(result.ErrorMessage);
            
            if (result.ErrorMessage?.Contains("already exists") == true)
                return BadRequest(result.ErrorMessage);
            
            return StatusCode(500, result.ErrorMessage);
        }

        _logger.LogInformation("User updated successfully: {Email}", updateUserDto.Email);
        return Ok(result.User);
    }

    /// <summary>
    /// Soft delete user
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var command = new DeleteUserCommand { Id = id };
        var result = await _deleteUserHandler.Handle(command);

        if (!result.Success)
        {
            _logger.LogError("Error deleting user with ID {UserId}: {ErrorMessage}", id, result.ErrorMessage);
            
            if (result.ErrorMessage?.Contains("not found") == true)
                return NotFound(result.ErrorMessage);
            
            return StatusCode(500, result.ErrorMessage);
        }

        _logger.LogInformation("User soft deleted successfully with ID: {UserId}", id);
        return NoContent();
    }

    /// <summary>
    /// Activate/Deactivate user
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> ToggleUserStatus(int id, [FromBody] bool isActive)
    {
        var command = new ToggleUserStatusCommand { Id = id, IsActive = isActive };
        var result = await _toggleUserStatusHandler.Handle(command);

        if (!result.Success)
        {
            _logger.LogError("Error updating user status with ID {UserId}: {ErrorMessage}", id, result.ErrorMessage);
            
            if (result.ErrorMessage?.Contains("not found") == true)
                return NotFound(result.ErrorMessage);
            
            return StatusCode(500, result.ErrorMessage);
        }

        _logger.LogInformation("User status updated with ID {UserId} - Active: {IsActive}", id, isActive);
        return Ok(new { Message = $"User {(isActive ? "activated" : "deactivated")} successfully" });
    }

    /// <summary>
    /// Confirm user email
    /// </summary>
    [HttpPatch("{id}/confirm-email")]
    public async Task<ActionResult> ConfirmEmail(int id)
    {
        var command = new ConfirmEmailCommand { Id = id };
        var result = await _confirmEmailHandler.Handle(command);

        if (!result.Success)
        {
            _logger.LogError("Error confirming email for user with ID {UserId}: {ErrorMessage}", id, result.ErrorMessage);
            
            if (result.ErrorMessage?.Contains("not found") == true)
                return NotFound(result.ErrorMessage);
            
            return StatusCode(500, result.ErrorMessage);
        }

        _logger.LogInformation("Email confirmed for user with ID: {UserId}", id);
        return Ok(new { Message = "Email confirmed successfully" });
    }

    /// <summary>
    /// Check if user exists
    /// </summary>
    [HttpGet("{id}/exists")]
    public async Task<ActionResult<bool>> CheckUserExists(int id)
    {
        var query = new CheckUserExistsQuery { Id = id };
        var result = await _checkUserExistsHandler.Handle(query);

        if (!result.Success)
        {
            _logger.LogError("Error checking user existence with ID {UserId}: {ErrorMessage}", id, result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        return Ok(result.Exists);
    }

    /// <summary>
    /// Check if email exists
    /// </summary>
    [HttpGet("email/{email}/exists")]
    public async Task<ActionResult<bool>> CheckEmailExists(string email)
    {
        var query = new CheckEmailExistsQuery { Email = email };
        var result = await _checkEmailExistsHandler.Handle(query);

        if (!result.Success)
        {
            _logger.LogError("Error checking email existence {Email}: {ErrorMessage}", email, result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        return Ok(result.Exists);
    }
}