using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Commons
{
    public class QuestionAndAnswer
    {
        public int Id { set; get; }
        public String QuestionContent { set; get; }
        public int QuestionType{ set; get; }
        public String A{ set; get; }
        public String B{ set; get; }
        public String C{ set; get; }
        public String D{ set; get; }
        public String AnswerExplain{ set; get; }
        public String CorrectAnswers{ set; get; }
        public String AnswerChose{ set; get; }
    }
}
