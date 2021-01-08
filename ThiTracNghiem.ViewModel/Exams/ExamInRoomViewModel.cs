using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Exams
{
    public class ExamInRoomViewModel
    {
        public int IdRoom { get; set; }
        public string RoomName { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeLimit { get; set; }
        public int Id { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDescription { get; set; }
        public int? ExamtypeId { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Action { get; set; }
        public string  RoomCode { get; set; }
    }
}
