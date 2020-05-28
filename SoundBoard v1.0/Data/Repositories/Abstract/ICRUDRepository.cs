using System.Collections.Generic;
using System.Threading.Tasks;
using Enteties.Abstract;

namespace Data.Repositories.Abstract
{
    public interface ICRUDRepository<TEntity> where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task RemoveByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity> FindByIdAsync(int id);
        Task UpdateAsync(TEntity entity);
    }
}
