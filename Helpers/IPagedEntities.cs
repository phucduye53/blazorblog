using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blazorblog.Helpers;
using Microsoft.EntityFrameworkCore;

namespace blazorblog.Helpers
{
    public interface IPagedEntities<out T>
    {
         int Page {get;}
        int TotalSize { get; }
        
        int PageSize { get; }
        IEnumerable<T> Entities { get; }
    }

    public class PagedEntities<T> : IPagedEntities<T>
    {

        public int Page {get;}
        public int TotalSize {get;}
        public int  PageSize { get; }
        public IEnumerable<T> Entities {get;}

        public PagedEntities()
        {
         
        }
        public PagedEntities(int page,int pageSize,int totalSize ,List<T> entities)
        {
            Page = page;
            PageSize = pageSize;
            TotalSize = totalSize;
            Entities = Entities;
        }
    }

}
