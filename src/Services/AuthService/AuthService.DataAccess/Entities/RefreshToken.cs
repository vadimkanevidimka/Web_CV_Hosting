namespace AuthService.DataAccess.Entities;

public class RefreshToken
{
    public Guid Key { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ExpirationTime { get; set; }

    public bool IsActive => DateTime.UtcNow < ExpirationTime;
}