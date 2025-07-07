using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Unleashed_Shared_UI.TagHelpers
{
    [HtmlTargetElement("form-input", Attributes = "asp-for")]
    public class FormInputTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public string Type { get; set; } = "text";
        public string Placeholder { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "form-group mb-4");

            var label = $"<label class='form-label' for='{For.Name}'>{For.Metadata.DisplayName ?? For.Name}</label>";
            var input = $"<input type='{Type}' id='{For.Name}' name='{For.Name}' placeholder='{Placeholder}' class='form-control' />";
            var validation = $"<span asp-validation-for='{For.Name}' class='text-danger'></span>";

            output.Content.AppendHtml(label);
            output.Content.AppendHtml(input);
            output.Content.AppendHtml(validation);
        }
    }
}