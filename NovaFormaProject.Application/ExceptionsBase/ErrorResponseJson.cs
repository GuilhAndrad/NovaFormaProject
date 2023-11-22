namespace NovaFormaProject.Application.ExceptionsBase;
public class ErrorResponseJson
{
    public List<string> ErrorMensages { get; set; }

    public ErrorResponseJson(string errorMensage)
    {
        ErrorMensages = new List<string>
        {
            errorMensage
        };
    }

    public ErrorResponseJson(List<string> errorMensage)
    {
        ErrorMensages = errorMensage;
    }
}
