using Data.Models;

namespace WebApi.Models;

public class UserResponceModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public bool? HasDriverLicense { get; set; }
    public List<Role> Roles { get; set; } = new();
}
