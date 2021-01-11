using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Questions
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public string ModuleName { get; set; }
        public int QuestionType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Action { get; set; }
    }
}
