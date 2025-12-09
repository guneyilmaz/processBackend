namespace ProcessModule.Application.Interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtTokenAsync(string email, IEnumerable<string> roles);
    Task<bool> ValidateUserAsync(string email, string password);
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string hashedPassword);
}