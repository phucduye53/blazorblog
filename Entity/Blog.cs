using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using blazorblog.Data.Dto;
using blazorblog.Entity.AbstractClass;
using blazorblog.Helpers;

namespace blazorblog.Entity
{
    public class Blog : AuditableEntity<int>
    {
        public string Title {get;set;}

        public string NormalizeTitle {get;set;}

        public string Content {get;set;}

        public string Summary {get;set;}

        public ICollection<BlogCategory> Categories { get; set; }


        public Blog(){}
        public Blog(BlogDto dto, IEnumerable<int> BlogCatagories)
        {
     
            Title = dto.Title;
      
            NormalizeTitle = Method.CustomNormalized(dto.Title);
            Content = dto.Content;
            Summary=dto.Summary;
            if(BlogCatagories == null)
            {
                Categories = null;
            }else{
                Categories = new List<BlogCategory>();
                foreach(var blogCategory in BlogCatagories)
                {
                    var newBlogCategory = new BlogCategory(this.Id,blogCategory);
                    Categories.Add(newBlogCategory);
                }
            }
        }
        public Blog Update(BlogDto dto, IEnumerable<int> BlogCatagories)
        {
            Title = dto.Title;
      
            NormalizeTitle = Method.CustomNormalized(dto.Title);
            Content = dto.Content;
            Summary=dto.Summary;
            if(BlogCatagories == null)
            {
                Categories = null;
            }else{
                Categories = new List<BlogCategory>();
                foreach(var blogCategory in BlogCatagories)
                {
                    var newBlogCategory = new BlogCategory(this.Id,blogCategory);
                    Categories.Add(newBlogCategory);
                }
            }
            return this;
        }

    }
}