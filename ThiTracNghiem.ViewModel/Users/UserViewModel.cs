using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Users
{
    public class UserViewModel
    {
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
    }
}
