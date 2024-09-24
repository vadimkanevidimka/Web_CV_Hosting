namespace AuthService.Buisness.Dtos.Tokens;

public class Token
{
    public string Value { get; set; }
    public DateTime ExpiresIn { get; set; }
}