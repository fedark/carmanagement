using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.Models;

namespace Web.TagHelpers;

public class SelectFilterTagHelper : TagHelper
{
    public string Name { get; set; } = default!;
    public SelectList Items { get; set; } = default!;
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

        var select = new TagBuilder("select");
        select.Attributes.Add("name", Name);
        select.Attributes.Add("onchange", "this.form.submit()");

        foreach (var item in Items)
        {
            select.InnerHtml.AppendHtml(CreateOption(item.Text, item.Selected));
        }

        output.Content.AppendHtml(select);
    }

    private TagBuilder CreateOption(string value, bool selected = false)
    {
        var option = new TagBuilder("option");

        if (selected)
        {
            option.Attributes.Add("selected", "selected");
        }

        option.InnerHtml.SetContent(value);
        return option;
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
