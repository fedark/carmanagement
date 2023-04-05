namespace Data.Models;
public class UserRole
{
    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}
