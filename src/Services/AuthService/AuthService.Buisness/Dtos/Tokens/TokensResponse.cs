namespace AuthService.Buisness.Dtos.Tokens;

public class TokensResponse
{
    public Token AccessToken { get; set; }

    public Token RefreshToken { get; set; }
}