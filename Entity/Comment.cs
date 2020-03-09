using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using blazorblog.Entity.AbstractClass;

namespace blazorblog.Entity
{
    public class Comment :AuditableEntity<int>
    {
        public int? ParentCommentId { get; set; }

        public string Content { get; set; }
        public string CommentIPaddress { get; set; }
        public int BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }
        public string CommentUserId { get; set; }
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment ParentComment { get; set; }
        public ICollection<Comment> InverseParentComment { get; set; }
    }
}