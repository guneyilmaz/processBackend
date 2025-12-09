using ProcessModule.Application.DTOs;

namespace ProcessModule.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ToggleUserStatusAsync(int id, bool isActive);
    Task<bool> ConfirmUserEmailAsync(int id);
    Task<bool> UserExistsAsync(int id);
    Task<bool> EmailExistsAsync(string email);
}