using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Unleashed_Shared_UI.TagHelpers
{
    [HtmlTargetElement("btn")]
    public class ButtonTagHelper : TagHelper
    {
        public ButtonVariant Variant { get; set; } = ButtonVariant.Primary;
        public string Text { get; set; }
        public string Href { get; set; }
        public string Icon { get; set; }
        public bool IsSubmit { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            bool isLink = !string.IsNullOrEmpty(Href);
            output.TagName = isLink ? "a" : "button";

            var classList = new List<string> { "btn" };

            switch (Variant)
            {
                case ButtonVariant.Primary:
                    classList.Add("btn-primary");
                    break;
                case ButtonVariant.Outline:
                    classList.Add("btn-outline");
                    break;
                case ButtonVariant.Danger:
                    classList.Add("btn-danger");
                    break;
                case ButtonVariant.Subtle:
                    classList.Add("btn-subtle");
                    break;
            }

            output.Attributes.SetAttribute("class", string.Join(" ", classList));

            output.Content.SetContent(Text);

            if (!string.IsNullOrEmpty(Icon))
            {
                output.PreContent.AppendHtml($"<i class='icon {Icon}'></i>");
            }

            if (isLink)
            {
                output.Attributes.SetAttribute("href", Href);
            }
            else
            {
                output.Attributes.SetAttribute("type", IsSubmit ? "submit" : "button");
            }
        }
    }

    public enum ButtonVariant
    {
        Primary,
        Outline,
        Danger,
        Subtle
    }
}