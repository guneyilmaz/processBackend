using ProcessModule.Application.Features.Users.Queries;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.Users.Queries;

public class GetAllUsersHandler
{
    private readonly IUserService _userService;

    public GetAllUsersHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<GetAllUsersResult> Handle(GetAllUsersQuery query)
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return new GetAllUsersResult
            {
                Success = true,
                Users = users
            };
        }
        catch (Exception ex)
        {
            return new GetAllUsersResult
            {
                Success = false,
                ErrorMessage = "An error occurred while retrieving users"
            };
        }
    }
}

public class GetUserByIdHandler
{
    private readonly IUserService _userService;

    public GetUserByIdHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery query)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(query.Id);
            return new GetUserByIdResult
            {
                Success = true,
                User = user,
                ErrorMessage = user == null ? "User not found" : null
            };
        }
        catch (Exception ex)
        {
            return new GetUserByIdResult
            {
                Success = false,
                ErrorMessage = "An error occurred while retrieving the user"
            };
        }
    }
}

public class GetUserByEmailHandler
{
    private readonly IUserService _userService;

    public GetUserByEmailHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<GetUserByEmailResult> Handle(GetUserByEmailQuery query)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(query.Email);
            return new GetUserByEmailResult
            {
                Success = true,
                User = user,
                ErrorMessage = user == null ? "User not found" : null
            };
        }
        catch (Exception ex)
        {
            return new GetUserByEmailResult
            {
                Success = false,
                ErrorMessage = "An error occurred while retrieving the user"
            };
        }
    }
}

public class CheckUserExistsHandler
{
    private readonly IUserService _userService;

    public CheckUserExistsHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<CheckUserExistsResult> Handle(CheckUserExistsQuery query)
    {
        try
        {
            var exists = await _userService.UserExistsAsync(query.Id);
            return new CheckUserExistsResult
            {
                Success = true,
                Exists = exists
            };
        }
        catch (Exception ex)
        {
            return new CheckUserExistsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while checking user existence"
            };
        }
    }
}

public class CheckEmailExistsHandler
{
    private readonly IUserService _userService;

    public CheckEmailExistsHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<CheckEmailExistsResult> Handle(CheckEmailExistsQuery query)
    {
        try
        {
            var exists = await _userService.EmailExistsAsync(query.Email);
            return new CheckEmailExistsResult
            {
                Success = true,
                Exists = exists
            };
        }
        catch (Exception ex)
        {
            return new CheckEmailExistsResult
            {
                Success = false,
                ErrorMessage = "An error occurred while checking email existence"
            };
        }
    }
}