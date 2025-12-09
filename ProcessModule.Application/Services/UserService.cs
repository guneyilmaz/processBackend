using ProcessModule.Application.DTOs;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;

namespace ProcessModule.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public UserService(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(MapToUserDto);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user != null ? MapToUserDto(user) : null;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        return user != null ? MapToUserDto(user) : null;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if email already exists
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(createUserDto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

        // Hash password
        var hashedPassword = await _authService.HashPasswordAsync(createUserDto.Password);

        var user = new User
        {
            Email = createUserDto.Email,
            Password = hashedPassword,
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            PhoneNumber = createUserDto.PhoneNumber,
            IsEmailConfirmed = createUserDto.IsEmailConfirmed,
            IsActive = createUserDto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return MapToUserDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        // Check if email is being changed and if new email already exists
        if (user.Email != updateUserDto.Email)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(updateUserDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");
        }

        // Update user properties
        user.Email = updateUserDto.Email;
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.IsActive = updateUserDto.IsActive;
        user.IsEmailConfirmed = updateUserDto.IsEmailConfirmed;
        user.UpdatedAt = DateTime.UtcNow;

        // Update password if provided
        if (!string.IsNullOrEmpty(updateUserDto.Password))
        {
            user.Password = await _authService.HashPasswordAsync(updateUserDto.Password);
        }

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return MapToUserDto(user);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return false;

        // Soft delete
        user.IsDeleted = true;
        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ToggleUserStatusAsync(int id, bool isActive)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return false;

        user.IsActive = isActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ConfirmUserEmailAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            return false;

        user.IsEmailConfirmed = true;
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UserExistsAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user != null;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _unitOfWork.Users.EmailExistsAsync(email);
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            IsEmailConfirmed = user.IsEmailConfirmed,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };
    }
}