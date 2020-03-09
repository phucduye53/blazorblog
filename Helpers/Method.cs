using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace blazorblog.Helpers
{
    public static class Method
    {
        public static async Task<IPagedEntities<T>> EntityWithPaging<T>(this IOrderedQueryable<T> query,int totalSize, int page, int pageSize)
        {
   

            var entities = await query.Skip((page * pageSize)).Take(pageSize).ToListAsync();

            return new PagedEntities<T>(page, pageSize, totalSize, entities);
        }
    }
}