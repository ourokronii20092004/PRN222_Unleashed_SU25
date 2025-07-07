using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.TagHelpers
{
    // This will activate on any <input-field> tag that has an 'asp-for' attribute
    [HtmlTargetElement("input-field", Attributes = "asp-for")]
    public class InputFieldTagHelper : TagHelper
    {
        // This is where we will inject the powerful IHtmlGenerator service
        private readonly IHtmlGenerator _generator;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        // This attribute will be populated with the model expression (e.g., model.Username)
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        // Optional attributes to customize the input
        public string Type { get; set; }
        public string Placeholder { get; set; }

        // Constructor to get the IHtmlGenerator service
        public InputFieldTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "input-group");
            output.TagMode = TagMode.StartTagAndEndTag;

            // 1. Generate the <label>
            // This automatically uses the [Display(Name="...")] from your ViewModel
            var label = _generator.GenerateLabel(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                labelText: null, // Let it be inferred
                htmlAttributes: new { @class = "input-label" });

            // 2. Generate the <input>
            // This is smart and will use the model value and data type annotations
            var input = _generator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                For.Model,
                format: null,
                htmlAttributes: new
                {
                    @class = "input-field-control",
                    placeholder = Placeholder,
                    type = Type // Allows overriding the type (e.g., for "email", "tel")
                });

            // 3. Generate the validation <span>
            // This automatically wires up unobtrusive client-side validation
            var validation = _generator.GenerateValidationMessage(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                message: null,
                tag: null,
                htmlAttributes: new { }); // The class is added automatically

            // Append all the generated parts to the output
            output.Content.AppendHtml(label);
            output.Content.AppendHtml(input);
            output.Content.AppendHtml(validation);
        }
    }
}