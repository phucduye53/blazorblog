using Microsoft.AspNetCore.Http;

namespace blazorblog.Data
{
    public class UserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUser()
        {
            if(_context.HttpContext != null)
            {
                return _context.HttpContext.User?.Identity?.Name;
            }else{
                return null;
            }
        }
    }

}