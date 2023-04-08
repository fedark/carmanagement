namespace Web.Models;

public class CarListViewModel
{
    public IEnumerable<CarViewModel> Cars { get; set; } = default!;
    public CarSortViewModel? SortModel { get; set; }
}
