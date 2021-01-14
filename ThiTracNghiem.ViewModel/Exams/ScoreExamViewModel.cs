using System.Collections.Generic;
using ThiTracNghiem_ViewModel.Commons;
using ThiTracNghiem_ViewModel.JoinRoom;

namespace ThiTracNghiem_ViewModel.Exams
{
    public class ScoreExamViewModel
    {
        public int Score { set; get; }
        public JoinRoomViewModel Joinroom { set; get; }
        public string Message { set; get; }
        public List<QuestionAndAnswer> Answers { set; get; }
    }
}