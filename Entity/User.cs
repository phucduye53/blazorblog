using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using blazorblog.Entity.AbstractClass;
namespace blazorblog.Entity
{
  public class User : UserAuditableEntity
  {
    public string FullName { get; set; }
  }
}