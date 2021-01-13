using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.JoinRoom
{
   public class JoinRoomViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public double? Score { get; set; }
        public DateTime? TimeSubmitExam { get; set; }
        public string Mssv { set; get; }
        public string Email { set; get; }
        public string FullName { set; get; }

    }
}
