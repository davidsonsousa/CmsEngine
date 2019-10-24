using System.Collections.Generic;
using System.Text;
using CmsEngine.Application.EditModels;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CmsEngine.Ui.TagHelpers
{
    public class CheckboxListTagHelper : TagHelper
    {
        /// <summary>
        /// Checkbox name, used to group all checkboxes in the list
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Class to be assigned to the outer <div> container
        /// </summary>
        public string OuterContainerClass { get; set; }

        /// <summary>
        /// Class to be assigned to the inner <div> container
        /// </summary>
        public string InnerContainerClass { get; set; }

        /// <summary>
        /// Class to be assigned to the <label>
        /// </summary>
        public string LabelClass { get; set; }

        /// <summary>
        /// Class to be assigned to the <checkbox>
        /// </summary>
        public string InputClass { get; set; }

        /// <summary>
        /// Items to appear in the checkbox list
        /// </summary>
        public IEnumerable<CheckboxEditModel> Items { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Items == null)
            {
                return;
            }

            output.TagName = "div";
            output.Attributes.SetAttribute("class", OuterContainerClass);

            InnerContainerClass = !string.IsNullOrWhiteSpace(InnerContainerClass) ? $" class=\"{InnerContainerClass}\"" : "";
            LabelClass = !string.IsNullOrWhiteSpace(LabelClass) ? $" class=\"{LabelClass}\"" : "";
            InputClass = !string.IsNullOrWhiteSpace(InputClass) ? $" class=\"{InputClass}\"" : "";

            string isChecked;
            string isEnabled;
            var sb = new StringBuilder();

            foreach (var item in Items)
            {
                isChecked = item.Selected ? " checked" : "";
                isEnabled = item.Enabled ? "" : " disabled";
                sb.Append("<div").Append(InnerContainerClass).Append(isEnabled).Append(">");
                sb.Append("<label").Append(LabelClass).Append(">");
                sb.Append("<input type=\"checkbox\" id=\"").Append(item.Value).Append("\" value=\"").Append(item.Value).Append("\" name=\"")
                  .Append(Name).Append("\"").Append(InputClass).Append(isChecked).Append(">").Append(item.Label);
                sb.Append("</label></div>");
            }

            output.Content.SetHtmlContent(sb.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
