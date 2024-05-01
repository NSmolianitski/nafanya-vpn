using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Outline;

public class OutlineKey
{
    public int Id { get; set; }
    public string AccessUrl { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}