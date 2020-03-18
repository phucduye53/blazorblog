using blazorblog.Entity.AbstractClass;

namespace blazorblog.Entity
{
    public class BlogCategory:AuditableEntity<int>
    {
        public int BlogId { get; set; }
        public int CategoryId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Category Category { get; set; }

        public BlogCategory(){}

        public BlogCategory(int BlogId,int CategoryId)
        {
            this.BlogId = BlogId;
            this.CategoryId = CategoryId;
        }
    }
}