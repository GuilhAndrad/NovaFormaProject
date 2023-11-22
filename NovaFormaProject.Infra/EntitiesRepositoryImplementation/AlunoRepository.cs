using Microsoft.EntityFrameworkCore;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
using NovaFormaProject.Domain.Interfaces.GenericRepositoryInterface;
using NovaFormaProject.Infra.DataContext;
using System.Linq.Expressions;

namespace NovaFormaProject.Infra.EntitiesRepositoryImplementation;
public class AlunoRepository : IGenericRepository<Aluno>, IAlunoRepository
{
    private readonly AppDbContext _context;

    public AlunoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Aluno entity)
    {
        _context.Set<Aluno>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Aluno entity)
    {
        _context.Set<Aluno>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Aluno>> FindAsync(Expression<Func<Aluno, bool>> predicate)
    {
        return await _context.Set<Aluno>().Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<Aluno>> GetAllAsync()
    {
        return await _context.Set<Aluno>().ToListAsync();
    }

    public async Task<IEnumerable<Aluno>> GetAlunosByNameAsync(string name)
    {
        return await _context.Set<Aluno>().Where(a => a.Name.Contains(name)).ToListAsync();
    }

    public async Task<Aluno> GetByIdAsync(int id)
    {
        return await _context.Set<Aluno>().FindAsync(id);
    }

    public async Task UpdateAsync(Aluno entity)
    {
        _context.Set<Aluno>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
