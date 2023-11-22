using Microsoft.EntityFrameworkCore;
using NovaFormaProject.Domain.DatabaseEntities;
using NovaFormaProject.Domain.Interfaces.EntitiesRepositoryInterface;
using NovaFormaProject.Domain.Interfaces.GenericRepositoryInterface;
using NovaFormaProject.Infra.DataContext;
using System.Linq.Expressions;

namespace NovaFormaProject.Infra.EntitiesRepositoryImplementation;
public class PagamentoRepository : IGenericRepository<Pagamento>, IPagamentoRepository
{
    private readonly AppDbContext _context;

    public PagamentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Pagamento entity)
    {
        _context.Set<Pagamento>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pagamento entity)
    {
        _context.Set<Pagamento>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Pagamento>> FindAsync(Expression<Func<Pagamento, bool>> predicate)
    {
        return await _context.Set<Pagamento>().Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<Pagamento>> GetAllAsync()
    {
        return await _context.Set<Pagamento>().ToListAsync();
    }

    public async Task<IEnumerable<Pagamento>> GetPagamentosByAlunoAsync(int id)
    {
        return await _context.Set<Pagamento>().Where(p => p.AlunoID == id).ToListAsync();
    }

    public async Task<Pagamento> GetByIdAsync(int id)
    {
        return await _context.Set<Pagamento>().FindAsync(id);
    }

    public async Task UpdateAsync(Pagamento entity)
    {
        _context.Set<Pagamento>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
