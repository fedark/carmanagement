using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.TagHelpers;

public static class UrlHelperExtensions
{
    public static string? ActionWithCarState(this IUrlHelper urlHelper, string? action, CarStateModel state)
    {
        return urlHelper.Action(action, new
        {
            state.SortModel?.SortKey,
            Ascending = state.SortModel?.Ascending ?? true,
            Company = state.FilterModel.Company.SelectedValue,
            state.FilterModel.Model,
            state.FilterModel.Year,
            state.FilterModel.Displacement,
        });
    }
}
