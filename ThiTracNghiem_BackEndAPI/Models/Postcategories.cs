using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Postcategories
    {
        public Postcategories()
        {
            Posts = new HashSet<Posts>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int CategoryType { get; set; }

        public virtual ICollection<Posts> Posts { get; set; }
    }
}
