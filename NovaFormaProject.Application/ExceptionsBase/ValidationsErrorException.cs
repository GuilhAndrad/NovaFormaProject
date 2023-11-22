namespace NovaFormaProject.Application.ExceptionsBase;
public class ValidationsErrorException : NovaFormaProjectException
{
    public List<string> ErrorMensages { get; set; }

    public ValidationsErrorException(List<string> errorMensages)
    {
        ErrorMensages = errorMensages;
    }
}
