using System;
using System.ComponentModel.DataAnnotations;

namespace ThiTracNghiem_ViewModel.Users
{
    public class RegisterRequest
    {
        public string Email { set; get; }
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string WorkPlace { set; get; }
        [Phone]
        public string PhoneNumber { set; get; }
        public string Address { set; get; }
        public int Gender { set; get; }
        public int RoleId { set; get; }
        public DateTime JoinDate { set; get; }
        public byte Active { set; get; }
    }
}
