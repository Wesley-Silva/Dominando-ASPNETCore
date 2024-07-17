using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASPNETCoreMVC.Extensions
{
    public class EmailTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string? nomeClasse = "text-primary";
            string emailTo = context.AllAttributes["mailTo"].Value.ToString();
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "mailto:" + emailTo);
            output.Attributes.SetAttribute("class", nomeClasse);
            output.Content.SetContent("Envie-nos um email");
        }
    }
}
