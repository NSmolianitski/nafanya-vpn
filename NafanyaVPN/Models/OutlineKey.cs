namespace NafanyaVPN.Models;

public class OutlineKey
{
    public int Id { get; set; }
    public string AccessUrl { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
}