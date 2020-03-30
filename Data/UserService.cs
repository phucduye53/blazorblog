using System.Linq;
using System.Threading.Tasks;
using blazorblog.Context;
using blazorblog.Data.Dto;
using blazorblog.Helpers;
using Microsoft.EntityFrameworkCore;

namespace blazorblog.Data
{
    public class UserService
    {
        private readonly blogContext _context;
        public UserService(blogContext context)
        {
            _context = context;

        }
        public async Task<IPagedEntities<UserDto>> GetUsersAsync(string paramSearch, int page)
        {
            var query = _context.Users.Where(x => x.UserName.ToLower().Contains(paramSearch)
                 || x.Email.ToLower().Contains(paramSearch));

            var totalSize = await query.AsNoTracking().CountAsync();           

            var orderQuery = query.Select(p => new UserDto
            {
                Id = p.Id,
                UserName = p.UserName,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                PasswordHash = p.PasswordHash,
     
            }).OrderBy(p => p.Email);

            return await Method.EntityWithPaging<UserDto>(orderQuery, totalSize, page - 1, Constant.PageSize);

        }
    }
}