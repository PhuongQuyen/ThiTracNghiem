using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Rooms
{
   public class RoomViewModel

    {
        public int Id { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public int TotalQuestions { get; set; }
        public int CurrentQuestions { get; set; }
        public int TimeLimit { get; set; }
        public int UserCounts { get; set; }
        public byte? PublicRoom { get; set; }
        public string Action { get; set; }
        public int ExamId { get; set; }
    }
}
