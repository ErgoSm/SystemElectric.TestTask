using System.Linq.Expressions;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Services;

namespace SystemElectric.TestTask.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        event EventHandler<TEntityArgs<T>>? OnAddedEntry;
        Task AddEntry(T entry, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetEntries(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    }
}
