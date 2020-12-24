using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Users
    {
        public Users()
        {
            Posts = new HashSet<Posts>();
            Scores = new HashSet<Scores>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkPlace { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int? Gender { get; set; }
        public int RoleId { get; set; }
        public DateTime JoinDate { get; set; }
        public byte Active { get; set; }

        public virtual Roles Role { get; set; }
        public virtual ICollection<Posts> Posts { get; set; }
        public virtual ICollection<Scores> Scores { get; set; }
    }
}
