namespace NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
public interface IUnitOfWork : IDisposable
{
    Task Commit();
}
