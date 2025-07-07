using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.TagHelpers
{
    [HtmlTargetElement("animated-checkbox", Attributes = "asp-for")]
    public class AnimatedCheckboxTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "label";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.AddClass("animated-checkbox-wrapper", HtmlEncoder.Default);

            // --- Create the Checkbox Input ---
            var input = new TagBuilder("input");
            input.Attributes.Add("type", "checkbox");
            input.Attributes.Add("id", For.Name);
            input.Attributes.Add("name", For.Name);
            input.Attributes.Add("value", "true");
            input.AddCssClass("animated-checkbox-input");

            if (For.Model is bool modelValue && modelValue)
            {
                input.Attributes.Add("checked", "checked");
            }

            // --- Create the Label Text ---
            var labelText = new TagBuilder("span");
            labelText.InnerHtml.Append(For.Metadata.DisplayName ?? For.Name);

            // --- Append the input and text to the main label tag ---
            output.Content.AppendHtml(input);
            output.Content.AppendHtml(labelText);
        }
    }
}