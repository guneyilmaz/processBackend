using System.ComponentModel.DataAnnotations;

namespace ProcessModule.Application.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = string.Empty;
}

public class RegisterDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    public bool IsEmailConfirmed { get; set; } = false;
    public bool IsActive { get; set; } = true;
}

public class UpdateUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string? Password { get; set; } // Optional - only update if provided

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    public bool IsEmailConfirmed { get; set; }
    public bool IsActive { get; set; }
}