using Microsoft.AspNetCore.Identity;
using OnlineAuctionApp.Application.DTOs.Auth;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Entities;

namespace OnlineAuctionApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }

    public async Task<TokenResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            throw new InvalidOperationException("Password and confirm password do not match.");

        var allowedRoles = new[] { "Buyer", "Seller" };

        var selectedRole = allowedRoles.FirstOrDefault(role =>
            string.Equals(role, dto.Role, StringComparison.OrdinalIgnoreCase));

        if (selectedRole is null)
            throw new InvalidOperationException("Role must be either Buyer or Seller.");

        if (!await _roleManager.RoleExistsAsync(selectedRole))
            throw new InvalidOperationException($"{selectedRole} role does not exist. Please seed roles first.");

        var existingEmailUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingEmailUser is not null)
            throw new InvalidOperationException("Email is already registered.");

        var existingUsernameUser = await _userManager.FindByNameAsync(dto.Username);

        if (existingUsernameUser is not null)
            throw new InvalidOperationException("Username is already taken.");

        var user = new AppUser
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            UserName = dto.Username.Trim(),
            Email = dto.Email.Trim()
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(" | ", createResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, selectedRole);

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            var errors = string.Join(" | ", roleResult.Errors.Select(error => error.Description));
            throw new InvalidOperationException(errors);
        }

        return await _jwtService.GenerateTokenAsync(user);
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UsernameOrEmail)
                   ?? await _userManager.FindByEmailAsync(dto.UsernameOrEmail);

        if (user is null)
            throw new InvalidOperationException("Invalid username/email or password.");

        var passwordIsValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!passwordIsValid)
            throw new InvalidOperationException("Invalid username/email or password.");

        return await _jwtService.GenerateTokenAsync(user);
    }
}