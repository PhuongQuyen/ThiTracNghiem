using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.JoinRoom
{
    public class JoinRoomRequest
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string FullName { set; get; }
        public string Mssv { set; get; }
        public string Email { set; get; }
    }
}
