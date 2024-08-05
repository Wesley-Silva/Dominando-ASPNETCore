using System.Text.RegularExpressions;

namespace ASPNETCoreMVC.Extensions
{
    public class RouteSlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null) return null;

            // transformar all lowercase e mudança de case colocar (-)
            return Regex.Replace(
                value.ToString()!,
                    "([a-z])([A-Z])",
                "$1-$2",
                RegexOptions.CultureInvariant,
                TimeSpan.FromMilliseconds(100))
                .ToLowerInvariant();
        }
    }
}
