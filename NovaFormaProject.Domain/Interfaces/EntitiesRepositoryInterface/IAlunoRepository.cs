using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.Interfaces.GenericRepositoryInterface;

namespace NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
public interface IAlunoRepository : IGenericRepository<Aluno>
{
    Task<IEnumerable<Aluno>> GetAlunosByNameAsync(string name);
}
