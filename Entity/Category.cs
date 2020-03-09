using blazorblog.Data.Dto;
using blazorblog.Entity.AbstractClass;

namespace blazorblog.Entity
{
    public class Category :AuditableEntity<int>
    {
        public string Name {get;set;}
        public Category()
        {

        }
        public Category(CategoryDto dto)
        {
            this.Name = dto.Name;
        }

    }
}