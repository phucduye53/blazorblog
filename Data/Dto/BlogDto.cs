using System;
using System.Collections.Generic;

namespace blazorblog.Data.Dto
{
    public class BlogDto
    {
        public int Id {get;set;}
        public string Title {get;set;}

        public string NormalizeTitle {get;set;}

        public string Content {get;set;}

        public string Summary {get;set;}


        public ICollection<CategoryDto> Categories { get; set; }
        
        // public ICollection<Comment> Comments { get; set; }
        public string UserName {get;set;}

        public DateTime CreatedDate {get;set;}

    }
}