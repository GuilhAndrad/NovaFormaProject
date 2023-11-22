using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.Interfaces.GenericRepositoryInterface;

namespace NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
public interface IPagamentoRepository : IGenericRepository<Pagamento>
{
    Task<IEnumerable<Pagamento>> GetPagamentosByAlunoAsync(int id);
}
