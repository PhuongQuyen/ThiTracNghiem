using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Questions
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public int? ModuleId { get; set; }
        public int QuestionType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Action { get; set; }
    }
}
