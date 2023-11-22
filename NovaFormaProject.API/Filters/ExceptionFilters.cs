using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NovaFormaProject.Application.ExceptionsBase;
using NovaFormaProject.Application.Validations.ResourcesMensagesError;
using System.Net;

namespace NovaFormaProject.API.Filters;

public class ExceptionFilters : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NovaFormaProjectException)
        {
            DealWithNovaFormaProjectException(context);
        }
        else
        {
            ThrowUnknownError(context);
        }
    }

    private static void DealWithNovaFormaProjectException(ExceptionContext context)
    {
        if (context.Exception is NovaFormaProjectException)
        {
            DealWithValidationErrorsExceptions(context);
        }
    }

    private static void DealWithValidationErrorsExceptions(ExceptionContext context)
    {
        var erroValidationException = context.Exception as ValidationsErrorException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ErrorResponseJson(erroValidationException.ErrorMensages));

    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult((AllErrors.ERRO_DESCONHECIDO, context.Exception.Message));
    }
}
