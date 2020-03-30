using System.Linq;
using System.Threading.Tasks;
using blazorblog.Context;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.EntityFrameworkCore;

namespace blazorblog.Data
{
    public class LogService
    {
        private readonly blogContext _context;
        public LogService(blogContext context)
        {
            _context = context;

        }

        public async Task<IPagedEntities<Log>> GetLogsAsync(int page)
        {
            var query = _context.Logs;

            var totalSize = await query.AsNoTracking().CountAsync();

            var orderQuery = query.OrderBy(p => p.LogDate);

            return await Method.EntityWithPaging<Log>(orderQuery, totalSize, page - 1, Constant.PageSize);

        }
        public Task<bool> CreateLogAsync(Log log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}