using Core.Lib.Domain.Data;
using Core.Lib.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Lib.Repositories;

public class Repository<T> where T : class, IEntity
{
    protected readonly Context _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(Context context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Entidade com o id: {id} não encontrada.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }
}
