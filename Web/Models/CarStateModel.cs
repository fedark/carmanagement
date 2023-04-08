namespace Web.Models;

public class CarStateModel : ICloneable
{
    public CarSortViewModel? SortModel { get; set; }
    public CarFilterViewModel FilterModel { get; set; } = default!;

    public object Clone()
    {
        return new CarStateModel { FilterModel = (CarFilterViewModel)FilterModel.Clone(), SortModel = (CarSortViewModel?)SortModel?.Clone() };
    }
}
