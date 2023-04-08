using Microsoft.AspNetCore.Mvc;
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
    public CarSortViewModel? State { get; set; } = default!;
    public string? Action { get; set; }

    public SortHeaderTagHelper(IUrlHelperFactory urlFactory)
    {
        urlFactory_ = urlFactory;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";

        var urlHelper = urlFactory_.GetUrlHelper(ViewContext);
        var requestAscending = State is not null && Key == State.SortKey ? !State.Ascending : true;
        var url = urlHelper.Action(Action, new { SortKey = Key, Ascending = requestAscending });
        output.Attributes.Add("href", url);

        if (State is not null && Key == State.SortKey)
        {
            var i = new TagBuilder("i");
            i.AddCssClass("bi");

            if (State.Ascending)
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
