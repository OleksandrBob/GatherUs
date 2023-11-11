namespace GatherUs.Core.Errors;

public class FormattedError
{
    public string ErrorMessage { get; set; }

    public List<object> Args { get; set; }

    public FormattedError()
    {
    }

    public FormattedError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}