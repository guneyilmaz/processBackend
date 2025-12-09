using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.Users.Commands;

public class CreateUserCommand
{
    public CreateUserDto CreateUserDto { get; set; } = null!;
}

public class CreateUserResult
{
    public bool Success { get; set; }
    public UserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
}

public class UpdateUserCommand
{
    public int Id { get; set; }
    public UpdateUserDto UpdateUserDto { get; set; } = null!;
}

public class UpdateUserResult
{
    public bool Success { get; set; }
    public UserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
}

public class DeleteUserCommand
{
    public int Id { get; set; }
}

public class DeleteUserResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ToggleUserStatusCommand
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}

public class ToggleUserStatusResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ConfirmEmailCommand
{
    public int Id { get; set; }
}

public class ConfirmEmailResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}