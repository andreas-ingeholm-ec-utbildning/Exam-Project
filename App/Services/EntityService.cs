using System.Linq.Expressions;
using App.DB;
using App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Services;

public class EntityService<T>(DBContext dbContext) where T : class, IEntity
{
    public Task<T?> Get(Guid id) =>
        Get(t => t.Id == id);

    public async Task<T?> Get(Expression<Func<T, bool>> expression) =>
        await dbContext.Set<T>().FirstOrDefaultAsync(expression);

    public async Task<IEnumerable<T>> List() =>
        await dbContext.Set<T>().ToArrayAsync();

    public async Task<IEnumerable<T>> List(Expression<Func<T, bool>> expression) =>
        await dbContext.Set<T>().Where(expression).ToArrayAsync();

    public async Task Add(T entity)
    {
        dbContext.Set<T>().Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async void Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async void Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> Exists(Guid id) =>
        await Get(id) is not null;

    public async Task<bool> Exists(Expression<Func<T, bool>> expression) =>
        await dbContext.Set<T>().FirstOrDefaultAsync(expression) is not null;
}
