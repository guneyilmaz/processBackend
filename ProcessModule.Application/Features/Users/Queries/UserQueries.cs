using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Features.Users.Queries;

public class GetAllUsersQuery
{
    // No parameters needed for getting all users
}

public class GetAllUsersResult
{
    public bool Success { get; set; }
    public IEnumerable<UserDto>? Users { get; set; }
    public string? ErrorMessage { get; set; }
}

public class GetUserByIdQuery
{
    public int Id { get; set; }
}

public class GetUserByIdResult
{
    public bool Success { get; set; }
    public UserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
}

public class GetUserByEmailQuery
{
    public string Email { get; set; } = string.Empty;
}

public class GetUserByEmailResult
{
    public bool Success { get; set; }
    public UserDto? User { get; set; }
    public string? ErrorMessage { get; set; }
}

public class CheckUserExistsQuery
{
    public int Id { get; set; }
}

public class CheckUserExistsResult
{
    public bool Success { get; set; }
    public bool Exists { get; set; }
    public string? ErrorMessage { get; set; }
}

public class CheckEmailExistsQuery
{
    public string Email { get; set; } = string.Empty;
}

public class CheckEmailExistsResult
{
    public bool Success { get; set; }
    public bool Exists { get; set; }
    public string? ErrorMessage { get; set; }
}