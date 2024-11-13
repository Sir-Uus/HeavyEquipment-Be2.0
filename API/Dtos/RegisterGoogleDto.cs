using System;

namespace API.Dtos;

public class RegisterGoogleDto
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Contact { get; set; } // jika ada
    public string Role { get; set; } = "User";
}
