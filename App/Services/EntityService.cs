using System.Linq.Expressions;
using App.DB;
using App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Services;

public class EntityService<T>(UserContext userContext) where T : Entity
{
    public Task<T?> Get(string id) =>
        Get(t => t.Id == id);

    public async Task<T?> Get(Expression<Func<T, bool>> expression) =>
        await userContext.Set<T>().FirstOrDefaultAsync(expression);

    public async Task<IEnumerable<T>> List() =>
        await userContext.Set<T>().ToArrayAsync();

    public async Task<IEnumerable<T>> List(Expression<Func<T, bool>> expression) =>
        await userContext.Set<T>().Where(expression).ToArrayAsync();

    public void Update(T entity) =>
        userContext.Set<T>().Update(entity);

    public void Delete(T entity)
    {
        userContext.Set<T>().Remove(entity);
    }

    public async Task<bool> Exists(string id)
    {
        return await Get(id) is not null;
    }
}
