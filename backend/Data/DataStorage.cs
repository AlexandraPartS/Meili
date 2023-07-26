using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class DataStorage<T> where T : class
    {
        private readonly UserContext _db;
        public DataStorage(UserContext dbcontext) {
            _db = dbcontext;
        }

        public async Task Save(T objectToSave)
        {
            _db.Add(objectToSave);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<DbSet<T>> Get()
        {
            return _db.Set<T>();
        }

        public async Task<T> Get(long id)
        {
            return await _db.FindAsync<T>(id).ConfigureAwait(false);
        }

        public async Task Update(T objectToSave)
        {
            _db.Update(objectToSave);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
