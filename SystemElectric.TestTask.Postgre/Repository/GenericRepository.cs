using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SystemElectric.TestTask.Domain.Args;
using SystemElectric.TestTask.Domain.Repositories;
using SystemElectric.TestTask.Postgre.Context;

namespace SystemElectric.TestTask.Postgre.Repository
{
    public sealed class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MainContext _context;

        public GenericRepository(MainContext context)
        {
            _context = context;
        }

        public event EventHandler<TEntityArgs<T>>? OnAddedEntry;

        public async Task AddEntry(T entry, CancellationToken cancellationToken)
        {
            _context.Set<T>().Add(entry);
            await _context.SaveChangesAsync(cancellationToken);

            OnAddedEntry?.Invoke(this, new TEntityArgs<T>(entry));
        }

        public async Task<IEnumerable<T>> GetEntries(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().Where(predicate).ToArrayAsync(cancellationToken);
        }
    }
}
