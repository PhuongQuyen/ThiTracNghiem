using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Rooms
{
    public class ExamInRoom
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public int QuestionType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Action { get; set; }
    }
}
