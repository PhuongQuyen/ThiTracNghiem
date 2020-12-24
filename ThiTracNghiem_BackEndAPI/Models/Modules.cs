using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Modules
    {
        public Modules()
        {
            Questions = new HashSet<Questions>();
        }

        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }

        public virtual ICollection<Questions> Questions { get; set; }
    }
}
