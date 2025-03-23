using Microsoft.EntityFrameworkCore;

namespace Watcher.Data.SQL.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IBaseRepository<T> WithContext(SqlDbContext context);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpsertAsync(List<T> entities);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly SqlDbContext _context;

        public BaseRepository(SqlDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual IBaseRepository<T> WithContext(SqlDbContext context)
        {
            return new BaseRepository<T>(context);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpsertAsync(List<T> entities)
        {
            if (entities == null || !entities.Any()) return;

            var entityKeys = entities.Where(e => EF.Property<int>(e, "RowId") != 0)
                                     .Select(e => EF.Property<int>(e, "RowId")).ToList();

            var existingEntities = await _context.Set<T>()
                .Where(e => entityKeys.Contains(EF.Property<int>(e, "RowId")))
                .ToListAsync();

            var existingEntitiesDict = existingEntities.ToDictionary(e => EF.Property<int>(e, "RowId"));

            var toUpdate = new List<T>();
            var toCreate = new List<T>();

            foreach (var entity in entities)
            {
                var key = EF.Property<int>(entity, "RowId");

                if (existingEntitiesDict.TryGetValue(key, out var existingEntity))
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    toUpdate.Add(existingEntity);
                }
                else
                {
                    toCreate.Add(entity);
                }
            }

            if (toUpdate.Any()) _context.Set<T>().UpdateRange(toUpdate);
            if (toCreate.Any()) await _context.Set<T>().AddRangeAsync(toCreate);

            await _context.SaveChangesAsync();
        }
    }
}
