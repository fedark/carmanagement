namespace Data.Models;
public class Model
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public int Year { get; set; }
    public Company Company { get; set; } = default!;
}
