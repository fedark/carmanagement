using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.Models;

namespace Web.TagHelpers;

public class FilterTagHelper : TagHelper
{
    public string Type { get; set; } = "text";
    public string Name { get; set; } = default!;
    public object? Value { get; set; }
    public CarStateModel State { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "form";
        output.Attributes.Add("method", "get");

        AppendInput(output, "sortKey", State.SortModel?.SortKey);
        AppendInput(output, "ascending", State.SortModel?.Ascending);
        AppendInput(output, "company", State.FilterModel.Company.SelectedValue);
        AppendInput(output, "model", State.FilterModel.Model);
        AppendInput(output, "year", State.FilterModel.Year);
        AppendInput(output, "displacement", State.FilterModel.Displacement);

        output.Content.AppendHtml(CreateInput(Type, Name, Value));

        var submit = new TagBuilder("input");
        submit.Attributes.Add("type", "submit");
        submit.Attributes.Add("value", "Filter");
        output.Content.AppendHtml(submit);
    }

    private TagBuilder CreateInput(string type, string name, object? value)
    {
        var input = new TagBuilder("input");
        input.Attributes.Add("type", type);
        input.Attributes.Add("name", name);
        input.Attributes.Add("value", value?.ToString());
        return input;
    }

    private TagBuilder CreateHidden(string name, object? value)
    {
        return CreateInput("hidden", name, value);
    }

    private void AppendInput(TagHelperOutput output, string name, object? value) 
    {
        if (!name.Equals(Name, StringComparison.OrdinalIgnoreCase))
        {
            output.Content.AppendHtml(CreateHidden(name, value));
        }
    }
}
