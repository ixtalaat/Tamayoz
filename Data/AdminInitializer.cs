using Microsoft.AspNetCore.Identity;

namespace Tamayoz.Data;

public static class AdminInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
    {
        var email = configuration["Admin:Email"] ?? "altamayozacademy640@gmail.com";
        var password = !string.IsNullOrWhiteSpace(configuration["Admin:Password"])
            ? configuration["Admin:Password"]!
            : "Admin@Tamayoz2026!";

        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        var users = services.GetRequiredService<UserManager<IdentityUser>>();
        var hasher = services.GetRequiredService<IPasswordHasher<IdentityUser>>();

        if (!await roles.RoleExistsAsync("Admin")) await roles.CreateAsync(new IdentityRole("Admin"));

        var user = await users.FindByEmailAsync(email) ?? await users.FindByNameAsync(email);
        if (user is null)
        {
            user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true, NormalizedEmail = email.ToUpperInvariant(), NormalizedUserName = email.ToUpperInvariant() };
            user.PasswordHash = hasher.HashPassword(user, password);
            user.SecurityStamp = Guid.NewGuid().ToString();
            var result = await users.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Could not create the configured Admin user: " + string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        if (!await users.IsInRoleAsync(user, "Admin")) await users.AddToRoleAsync(user, "Admin");

    }

}
