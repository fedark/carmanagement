namespace Data.Models;
public class Car
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int? Displacement { get; set; }
    public byte[] Picture { get; set; } = default!;
    public string PictureType { get; set; } = default!;
    public Model Model { get; set; } = default!;
}
