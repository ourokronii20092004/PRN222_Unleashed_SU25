using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.TagHelpers
{
    // Activates on a <modal-confirm> tag
    [HtmlTargetElement("modal-confirm")]
    public class ModalConfirmTagHelper : TagHelper
    {
        // --- Tag Helper Attributes ---
        public string ModalId { get; set; }
        public string TriggerButtonText { get; set; } = "Delete";
        public ButtonVariant TriggerButtonVariant { get; set; } = ButtonVariant.Danger;
        public string ModalTitle { get; set; } = "Confirm Action";
        public string ItemName { get; set; } // The name of the item for the message

        // --- Attributes for the form post ---
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteData { get; set; } = new Dictionary<string, string>();

        private readonly IHtmlGenerator _generator;
        public ModalConfirmTagHelper(IHtmlGenerator generator) { _generator = generator; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null; // We are generating a complete structure, so don't render the <modal-confirm> tag itself.

            var triggerButton = RenderTriggerButton();
            var modal = RenderModal();

            output.Content.AppendHtml(triggerButton);
            output.Content.AppendHtml(modal);
        }

        private string RenderTriggerButton()
        {
            return $@"<button type='button' class='btn btn-{TriggerButtonVariant.ToString().ToLower()}' data-modal-trigger data-modal-target='{ModalId}'>
                          {TriggerButtonText}
                      </button>";
        }

        private string RenderModal()
        {
            var formAction = $"/{Controller}/{Action}";
            if (RouteData.Any())
            {
                var routeValues = string.Join("/", RouteData.Values);
                formAction += $"/{routeValues}";
            }

            var modalHtml = new StringBuilder();
            modalHtml.Append($@"<div class='modal-container' id='{ModalId}' aria-hidden='true'>
                <div class='modal-overlay' data-modal-close></div>
                <div class='modal-content' role='dialog' aria-modal='true'>
                    <div class='modal-header'>
                        <h2>{ModalTitle}</h2>
                    </div>
                    <div class='modal-body'>
                        <p>Are you sure you want to delete <strong class='text-danger'>""{ItemName}""</strong>? This action cannot be undone.</p>
                    </div>
                    <div class='modal-footer'>
                        <button type='button' class='btn btn-outline' data-modal-close>Cancel</button>
                        <form action='{formAction}' method='post'>
                            {RenderAntiforgeryToken()}
                            <button type='submit' class='btn btn-danger'>Delete</button>
                        </form>
                    </div>
                </div>
            </div>");

            return modalHtml.ToString();
        }

        // Helper to generate the anti-forgery token for security
        private string RenderAntiforgeryToken()
        {
            return _generator.GenerateAntiforgery(null).ToString();
        }
    }
}