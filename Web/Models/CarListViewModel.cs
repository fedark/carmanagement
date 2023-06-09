﻿namespace Web.Models;

public class CarListViewModel
{
    public IEnumerable<CarViewModel> Cars { get; set; } = default!;
    public CarStateModel State { get; set; } = default!;
}
