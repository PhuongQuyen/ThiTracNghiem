using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Rooms
{
    public class RoomRequest
    {
        public byte? PublicRoom { get; set; }
        public string RoomCode { get; set; }
        public int UserId { get; set; }
        public string RoomName { get; set; }
        public string Description { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeLimit { get; set; }
    }
}
