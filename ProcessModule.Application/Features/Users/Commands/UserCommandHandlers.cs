using ProcessModule.Application.Features.Users.Commands;
using ProcessModule.Application.Interfaces;

namespace ProcessModule.Application.Features.Users.Commands;

public class CreateUserHandler
{
    private readonly IUserService _userService;

    public CreateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<CreateUserResult> Handle(CreateUserCommand command)
    {
        try
        {
            var user = await _userService.CreateUserAsync(command.CreateUserDto);
            return new CreateUserResult
            {
                Success = true,
                User = user
            };
        }
        catch (InvalidOperationException ex)
        {
            return new CreateUserResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception ex)
        {
            return new CreateUserResult
            {
                Success = false,
                ErrorMessage = "An error occurred while creating the user"
            };
        }
    }
}

public class UpdateUserHandler
{
    private readonly IUserService _userService;

    public UpdateUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UpdateUserResult> Handle(UpdateUserCommand command)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(command.Id, command.UpdateUserDto);
            return new UpdateUserResult
            {
                Success = true,
                User = user
            };
        }
        catch (KeyNotFoundException ex)
        {
            return new UpdateUserResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
        catch (InvalidOperationException ex)
        {
            return new UpdateUserResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception ex)
        {
            return new UpdateUserResult
            {
                Success = false,
                ErrorMessage = "An error occurred while updating the user"
            };
        }
    }
}

public class DeleteUserHandler
{
    private readonly IUserService _userService;

    public DeleteUserHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<DeleteUserResult> Handle(DeleteUserCommand command)
    {
        try
        {
            var success = await _userService.DeleteUserAsync(command.Id);
            return new DeleteUserResult
            {
                Success = success,
                ErrorMessage = success ? null : "User not found"
            };
        }
        catch (Exception ex)
        {
            return new DeleteUserResult
            {
                Success = false,
                ErrorMessage = "An error occurred while deleting the user"
            };
        }
    }
}

public class ToggleUserStatusHandler
{
    private readonly IUserService _userService;

    public ToggleUserStatusHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ToggleUserStatusResult> Handle(ToggleUserStatusCommand command)
    {
        try
        {
            var success = await _userService.ToggleUserStatusAsync(command.Id, command.IsActive);
            return new ToggleUserStatusResult
            {
                Success = success,
                ErrorMessage = success ? null : "User not found"
            };
        }
        catch (Exception ex)
        {
            return new ToggleUserStatusResult
            {
                Success = false,
                ErrorMessage = "An error occurred while updating user status"
            };
        }
    }
}

public class ConfirmEmailHandler
{
    private readonly IUserService _userService;

    public ConfirmEmailHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ConfirmEmailResult> Handle(ConfirmEmailCommand command)
    {
        try
        {
            var success = await _userService.ConfirmUserEmailAsync(command.Id);
            return new ConfirmEmailResult
            {
                Success = success,
                ErrorMessage = success ? null : "User not found"
            };
        }
        catch (Exception ex)
        {
            return new ConfirmEmailResult
            {
                Success = false,
                ErrorMessage = "An error occurred while confirming email"
            };
        }
    }
}