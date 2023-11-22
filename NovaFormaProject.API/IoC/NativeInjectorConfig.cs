using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NovaFormaProject.API.Filters;
using NovaFormaProject.Application.Extensions;
using NovaFormaProject.Application.Services.AlunoUseCase;
using NovaFormaProject.Application.Services.PagamentoUseCase;
using NovaFormaProject.Application.Validations;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
using NovaFormaProject.Infra.DataContext;
using NovaFormaProject.Infra.EntitiesRepositoryImplementation;

namespace NovaFormaProject.API.IoC;

public static class NativeInjectorConfig
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        //connection
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

        //registro dos repositorios
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //validator
        services.AddTransient<AlunoValidator>();
        services.AddTransient<PagamentoValidator>();

        // Registro do AlunoServices e PagamentoServices
        services.AddTransient<AlunoServices>();
        services.AddTransient<PagamentoServices>();
        services.AddHostedService<PagamentoUpdateService>();

        //registro do autoMapper
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);

        //registro do filtro de execeções
        services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilters)));

    }
}
