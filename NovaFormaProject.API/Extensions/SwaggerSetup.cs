using Microsoft.OpenApi.Models;
using System.Reflection;

namespace NovaFormaProject.API.Extensions;

public static class SwaggerSetup
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {

            // Configuração para incluir comentários XML na documentação
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NovaFormaAPI",
                Version = "v1",
                Description = "Uma api com o intuito de registrar alunos e pagamentos em uma academia. Pra resumir, identificar caloteiro.",
            });
        });
    }
}