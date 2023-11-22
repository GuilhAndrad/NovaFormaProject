using Microsoft.EntityFrameworkCore;
using NovaFormaProject.Domain.DatabaseEntities;
using System.Reflection;

namespace NovaFormaProject.Infra.DataContext;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
