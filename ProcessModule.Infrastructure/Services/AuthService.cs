using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProcessModule.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProcessModule.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public Task<string> GenerateJwtTokenAsync(string email, IEnumerable<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "ProcessModuleAPI";
        var audience = jwtSettings["Audience"] ?? "ProcessModuleClient";
        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email),
            new(ClaimTypes.NameIdentifier, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        // Add role claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<bool> ValidateUserAsync(string email, string password)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);
        if (user == null || !user.IsActive)
            return false;

        return await VerifyPasswordAsync(password, user.Password);
    }

    public async Task<string> HashPasswordAsync(string password)
    {
        return await Task.Run(() =>
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[32];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            var combined = new byte[64];
            Array.Copy(salt, 0, combined, 0, 32);
            Array.Copy(hash, 0, combined, 32, 32);

            return Convert.ToBase64String(combined);
        });
    }

    public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
    {
        return await Task.Run(() =>
        {
            try
            {
                var combined = Convert.FromBase64String(hashedPassword);
                if (combined.Length != 64)
                    return false;

                var salt = new byte[32];
                var hash = new byte[32];
                Array.Copy(combined, 0, salt, 0, 32);
                Array.Copy(combined, 32, hash, 0, 32);

                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
                var computedHash = pbkdf2.GetBytes(32);

                return hash.SequenceEqual(computedHash);
            }
            catch
            {
                return false;
            }
        });
    }
}