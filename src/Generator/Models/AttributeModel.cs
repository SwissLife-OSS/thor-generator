using Newtonsoft.Json;

namespace ChilliCream.Logging.Generator
{
    public class AttributeModel
    {
        [JsonProperty("argumentSyntax")]
        public string ArgumentSyntax { get; set; }
    }
}
