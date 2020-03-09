using System.ComponentModel.DataAnnotations.Schema;
using blazorblog.Entity.AbstractClass;

namespace blazorblog.Entity
{
    public class Blog : AuditableEntity<int>
    {
        public string Title {get;set;}

        public string NormalizeTitle {get;set;}

        public string Content {get;set;}

        public string Summary {get;set;}

        public int CategoryId {get;set;}
        [ForeignKey("CategoryId")]
        public Category Category {get;set;}

    }
}