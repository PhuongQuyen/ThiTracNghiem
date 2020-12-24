using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Posts
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostcategoryId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public string PostSlug { get; set; }
        public int Views { get; set; }
        public string Thumbnail { get; set; }
        public int Status { get; set; }
        public DateTime PostDate { get; set; }

        public virtual Postcategories Postcategory { get; set; }
        public virtual Users User { get; set; }
    }
}
