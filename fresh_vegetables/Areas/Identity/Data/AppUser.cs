using Microsoft.AspNetCore.Identity;

namespace fresh_vegetables.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    [PersonalData]
    public required string Name { get; set; }
    [PersonalData]
    public string Address { get; set; } = string.Empty;
    [PersonalData]
    public string ProfilePicture { get; set; } = string.Empty;
}

