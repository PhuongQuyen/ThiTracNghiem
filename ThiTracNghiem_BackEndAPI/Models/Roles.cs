using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string RoleTitle { get; set; }
        public string RoleDescription { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
