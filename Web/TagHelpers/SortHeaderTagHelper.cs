using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.Models;

namespace Web.TagHelpers;

public class SortHeaderTagHelper : TagHelper
{
    private readonly IUrlHelperFactory urlFactory_;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    public CarSortKey Key { get; set; } = default!;
    public CarStateModel State { get; set; } = default!;
    public string? Action { get; set; }

    public SortHeaderTagHelper(IUrlHelperFactory urlFactory)
    {
        urlFactory_ = urlFactory;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";

        var urlHelper = urlFactory_.GetUrlHelper(ViewContext);
        var requestAscending = Key == State.SortModel?.SortKey ? !State.SortModel.Ascending : true;

        var requestState = (CarStateModel)State.Clone();
        requestState.SortModel ??= new CarSortViewModel();
        requestState.SortModel.SortKey = Key;
        requestState.SortModel.Ascending = requestAscending;

        var url = urlHelper.ActionWithCarState(Action, requestState);
        output.Attributes.Add("href", url);

        if (Key == State.SortModel?.SortKey)
        {
            var i = new TagBuilder("i");
            i.AddCssClass("bi");

            if (State.SortModel.Ascending)
            {
                i.AddCssClass("bi-caret-up-fill");
            }
            else
            {
                i.AddCssClass("bi-caret-down-fill");
            }

            output.PreContent.AppendHtml(i);
            output.PreContent.Append(" ");
        }
    }
}
