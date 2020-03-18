using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blazorblog.Context;
using blazorblog.Data.Dto;
using blazorblog.Entity;
using blazorblog.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace blazorblog.Data
{
    public class BlogService
    {
        private readonly blogContext _context;
        private readonly UserManager<User> _userManager;
        public BlogService(blogContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public async Task<IPagedEntities<BlogDto>> GetBlogsAsync(BlogInputDto input)
        {

            IQueryable<Blog> query = _context.Blogs.Include(x => x.Categories);
            if (input.CategoryId != 0)
            {
                query = query.Where(p => p.Categories.Any(y => y.Id == input.CategoryId));
            }
            var totalSize = await query.AsNoTracking().CountAsync();

            var orderQuery = query.Select(p => new BlogDto
            {
                Id = p.Id,
                Title = p.Title,
                Summary = p.Summary,
                NormalizeTitle = p.NormalizeTitle,
                Content = p.Content,
                UserName = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Categories = p.Categories.Select(y => new CategoryDto
                {
                    CategoryId = y.Id,
                    Name = y.Category.Name
                }).ToList()
            }).OrderBy(p => p.CreatedDate);


            return await Method.EntityWithPaging<BlogDto>(orderQuery, totalSize, input.page - 1, Constant.PageSize);
        }

        public async Task<BlogDto> GetBlogAsync(string NormalizeTitle)
        {
            var query = await _context.Blogs.Include(x => x.Categories)
            .Where(p => p.NormalizeTitle == NormalizeTitle)
            .Select(p => new BlogDto
            {
                Id = p.Id,
                Title = p.Title,
                Summary = p.Summary,
                NormalizeTitle = p.NormalizeTitle,
                Content = p.Content,
                UserName = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Categories = p.Categories.Select(y => new CategoryDto
                {
                    CategoryId = y.Id,
                    Name = y.Category.Name
                }).ToList()
            }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<IPagedEntities<BlogDto>> GetBlogsAdminAsync(BlogInputDto input)
        {
            IQueryable<Blog> query = _context.Blogs.Include(x => x.Categories)
            .Where(p => p.CreatedBy == input.UserName);
            var totalSize = await query.AsNoTracking().CountAsync();

            var orderQuery = query.Select(p => new BlogDto
            {
                Id = p.Id,
                Title = p.Title,
                Summary = p.Summary,
                NormalizeTitle = p.NormalizeTitle,
                Content = p.Content,
                UserName = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Categories = p.Categories.Select(y => new CategoryDto
                {
                    CategoryId = y.Id,
                    Name = y.Category.Name
                }).ToList()
            }).OrderBy(p => p.CreatedDate);


            return await Method.EntityWithPaging<BlogDto>(orderQuery, totalSize, input.page - 1, Constant.PageSize);
        }

        public Task<Blog> CreateBlogAsync(BlogDto newBlog, IEnumerable<int> BlogCatagories)
        {

            Blog objBlog = new Blog(newBlog, BlogCatagories);
            objBlog.Id = 0;
            _context.Blogs.Add(objBlog);
            _context.SaveChanges();

            return Task.FromResult(objBlog);

        }

        public Task<bool> DeleteBlogAsync(int BlogId)
        {
            var ExistingBlogs =
                _context.Blogs
                .Where(x => x.Id == BlogId)
                .FirstOrDefault();

            if (ExistingBlogs != null)
            {
                _context.Blogs.Remove(ExistingBlogs);
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }


        public Task<bool> UpdateBlogAsync(BlogDto objBlogs, IEnumerable<int> BlogCategories)
        {

            var ExistingBlogs =
                _context.Blogs
                .Include(x => x.Categories)
                .Where(x => x.Id == objBlogs.Id)
                .FirstOrDefault();

            if (ExistingBlogs != null)
            {

                
                ExistingBlogs.Update(objBlogs,BlogCategories);

                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

    }

}
