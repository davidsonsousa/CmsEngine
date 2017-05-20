using Newtonsoft.Json;

namespace CmsEngine.Utils
{
    public class ReturnValue
    {
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "exception")]
        public string Exception { get; set; }

        [JsonProperty(PropertyName = "isError")]
        public bool IsError { get; set; }
    }
}
