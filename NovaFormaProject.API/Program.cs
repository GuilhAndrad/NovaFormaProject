using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NovaFormaProject.API.Filters;
using NovaFormaProject.Application.Extensions;
using NovaFormaProject.Application.Services;
using NovaFormaProject.Application.Validations;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
using NovaFormaProject.Infra.DataContext;
using NovaFormaProject.Infra.EntitiesRepositoryImplementation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//validator
builder.Services.AddTransient<AlunoValidator>();
builder.Services.AddTransient<PagamentoValidator>();
// Registro do AlunoServices
builder.Services.AddTransient<AlunoServices>();
builder.Services.AddTransient<PagamentoServices>();

builder.Services.AddHostedService<PagamentoUpdateService>();

builder.Services.AddCors();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddControllers()
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NovaFormaAPI",
        Version = "v1",
        Description = "Uma api com o intuito de registrar alunos e pagamentos em uma academia. Pra resumir, identificar caloteiro.",
    });

    // Configuração para incluir comentários XML na documentação
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//config do automapper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilters)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
        c.RoutePrefix = "swagger"; // Pode ser ajustado conforme necessário
    });
}

app.UseCors(opt => opt.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
