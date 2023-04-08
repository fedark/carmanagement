namespace Web.Models;

public class CarSortViewModel : ICloneable
{
    public CarSortKey SortKey { get; set; }
    public bool Ascending { get; set; } = true;

    public object Clone()
    {
        return new CarSortViewModel { SortKey = SortKey, Ascending = Ascending };
    }

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