using blazorblog.Entity.AbstractClass;

namespace blazorblog.Entity
{
    public class Category :AuditableEntity<int>
    {
        public string Name {get;set;}
    }
}