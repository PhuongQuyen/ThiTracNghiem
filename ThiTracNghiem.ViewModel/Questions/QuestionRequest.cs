using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Questions
{
    public class QuestionRequest
    {
        public string QuestionContent { get; set; }
        public int? ModuleId { get; set; }
        public int QuestionType { get; set; }
        public int ExamId { get; set; }
        public DateTime DateCreated { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string CorrectAnswers { get; set; }
        public string AnswerExplain { get; set; }
    }
}
