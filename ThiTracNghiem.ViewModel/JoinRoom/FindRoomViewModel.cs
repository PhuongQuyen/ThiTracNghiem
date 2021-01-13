using System;
using System.Collections.Generic;
using System.Text;
using ThiTracNghiem_ViewModel.Exams;

namespace ThiTracNghiem_ViewModel.JoinRoom
{
    public class FindRoomViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public byte? PublicRoom { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public ExamViewModel exam { set; get; }
    }
}
