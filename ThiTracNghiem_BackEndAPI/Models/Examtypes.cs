using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Examtypes
    {
        public Examtypes()
        {
            Exams = new HashSet<Exams>();
        }

        public int Id { get; set; }
        public string TypeTitle { get; set; }
        public string TypeDescription { get; set; }
        public string TypeSlug { get; set; }

        public virtual ICollection<Exams> Exams { get; set; }
    }
}
