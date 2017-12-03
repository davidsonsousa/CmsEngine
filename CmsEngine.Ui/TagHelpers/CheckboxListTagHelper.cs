using System.Collections.Generic;
using System.Text;
using CmsEngine.Data.EditModels;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CmsEngine.Ui.TagHelpers
{
    public class CheckboxListTagHelper : TagHelper
    {
        public string OuterContainerClass { get; set; }
        public string InnerContainerClass { get; set; }
        public string LabelClass { get; set; }
        public string InputClass { get; set; }
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
                sb.Append($"<div{InnerContainerClass}{isEnabled}>");
                sb.Append($"<label{LabelClass}>");
                sb.Append($"<input type=\"checkbox\" id=\"{item.Value}\" value=\"{item.Value}\"{InputClass}{isChecked}>{item.Label}");
                sb.Append("</label></div>");
            }

            output.Content.SetHtmlContent(sb.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
