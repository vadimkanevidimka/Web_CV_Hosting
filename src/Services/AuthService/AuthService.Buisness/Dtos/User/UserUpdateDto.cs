using System.Text.Json.Serialization;

namespace AuthService.Buisness.Dtos.User;

public class UserUpdateDto
{
    [JsonIgnore]
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}