using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Score
{
    public class TopScoreViewModel
    {
        public int ExamId { get; set; }
        public string UserName { get; set; }
        public string ExamTitle { get; set; }
        public int TotalQuestions { get; set; }
        public float TotalQuestionsCorrect { get; set; }
        public DateTime TestDate { get; set; }
    }
}
