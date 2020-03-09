using System.Collections.Generic;
using System.Threading.Tasks;
using blazorblog.Context;
using blazorblog.Data.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using blazorblog.Helpers;
using blazorblog.Entity;
using System;

namespace blazorblog.Data
{
    public class CategoryService
    {
        private readonly blogContext _context;
        public CategoryService(blogContext context)
        {
            _context = context;

        }
        public async Task<List<CategoryDto>> GetCategorysAsync()
        {
            return await (from category in _context.Categories
                          select new CategoryDto
                          {
                              CategoryId = category.Id,
                              Name = category.Name
                          }).AsNoTracking().OrderBy(p => p.Name).ToListAsync();
        }
        public async Task<IPagedEntities<CategoryDto>> GetCategoriesAsync(int page)
        {
            var query = _context.Categories;

            var totalSize = await query.AsNoTracking().CountAsync();

            var orderQuery = query.Select(p=> new CategoryDto{
                CategoryId = p.Id,
                Name = p.Name
            }).OrderBy(p=>p.Name);

            return await Method.EntityWithPaging<CategoryDto>(orderQuery, totalSize, page, Constant.PageSize);
        }



        public Task<bool> CreateCategoryAsync(CategoryDto dto)
        {

            Category category = new Category(dto);

            _context.Categories.Add(category);
            _context.SaveChanges();
            return Task.FromResult(true);


        }



        public Task<bool> UpdateCategoryAsync(CategoryDto dto)
        {

            int intCategoryId = Convert.ToInt32(dto.CategoryId);

            var ExistingCategory =
                _context.Categories
                .Where(x => x.Id == intCategoryId)
                .FirstOrDefault();

            if (ExistingCategory != null)
            {
                 ExistingCategory.Name =dto.Name;

                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);

        }



        public Task<bool> DeleteCategoryAsync(CategoryDto dto)
        {
           int intCategoryId = Convert.ToInt32(dto.CategoryId);

            var ExistingCategory =
                _context.Categories
                .Where(x => x.Id == intCategoryId)
                .FirstOrDefault();

            if (ExistingCategory != null)
            {
                _context.Categories.Remove(ExistingCategory);
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