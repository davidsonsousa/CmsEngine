namespace CmsEngine.Core.Utils;

public class ReturnValue
{
    public ReturnValue(string message, bool isError = false)
    {
        Message = message;
        IsError = isError;
    }

    [JsonPropertyName("message")]
    public string Message { get; private set; }

    [JsonPropertyName("isError")]
    public bool IsError { get; private set; }

    [JsonPropertyName("exception")]
    public string Exception { get; private set; } = string.Empty;

    //[JsonPropertyName("value")]
    //public object Value { get; set; }

    public void SetErrorMessage(string message, string exception = "")
    {
        Message = message;
        Exception = exception;
        IsError = true;
    }
}
