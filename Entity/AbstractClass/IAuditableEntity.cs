using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace blazorblog.Entity.AbstractClass
{
  public interface IAuditableEntity
    {
       DateTime CreatedDate { get; set; }
     
       string CreatedBy { get; set; }
 
       DateTime UpdatedDate { get; set; }
             
       string UpdatedBy { get; set; }
    }
    public abstract class AuditableEntity<T>: Entity<T>, IAuditableEntity  
    {
       [ScaffoldColumn(false)]
       public DateTime CreatedDate { get; set; }
 
      
       [MaxLength(256)]
       [ScaffoldColumn(false)]
       public string CreatedBy { get; set; }
 
       [ScaffoldColumn(false)]
       public DateTime UpdatedDate { get; set; }
 
       [MaxLength(256)]
       [ScaffoldColumn(false)]
       public string UpdatedBy { get; set; }
    }
    public abstract class UserAuditableEntity: IdentityUser, IAuditableEntity  
    {
       [ScaffoldColumn(false)]
       public DateTime CreatedDate { get; set; }
 
      
       [MaxLength(256)]
       [ScaffoldColumn(false)]
       public string CreatedBy { get; set; }
 
       [ScaffoldColumn(false)]
       public DateTime UpdatedDate { get; set; }
 
       [MaxLength(256)]
       [ScaffoldColumn(false)]
       public string UpdatedBy { get; set; }
    }
}