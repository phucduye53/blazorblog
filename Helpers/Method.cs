using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace blazorblog.Helpers
{
    public static class Method
    {
        public static async Task<IPagedEntities<T>> EntityWithPaging<T>(this IOrderedQueryable<T> query, int totalSize, int page, int pageSize)
        {


            var entities = await query.Skip((page * pageSize)).Take(pageSize).ToListAsync();

            return new PagedEntities<T>(page, pageSize, totalSize, entities);
        }
        public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> transform)
        {
            return condition ? transform(source) : source;
        }

        public static string CustomNormalized(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);
            StringBuilder resultBuilder = new StringBuilder();
            foreach (var character in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);
                if (category == UnicodeCategory.LowercaseLetter
              || category == UnicodeCategory.UppercaseLetter
             || category == UnicodeCategory.SpaceSeparator)
                    resultBuilder.Append(character);
            }
            string result = Regex.Replace(resultBuilder.ToString(), @"\s+", "-");
            return result;
        }
    }
}