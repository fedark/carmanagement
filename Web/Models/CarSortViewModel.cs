namespace Web.Models;

public class CarSortViewModel
{
    public CarSortKey SortKey { get; set; }
    public bool Ascending { get; set; } = true;

    public void Deconstruct(out CarSortKey Key, out bool Ascending)
    {
        Key = SortKey;
        Ascending = this.Ascending;
    }
}

public enum CarSortKey
{
    Company,
    Model,
    Year,
    Displacement
}