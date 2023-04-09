namespace Data.Models;
public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool? HasDriverLicense { get; set; }
    public List<Role> Roles { get; set; } = new();
}
