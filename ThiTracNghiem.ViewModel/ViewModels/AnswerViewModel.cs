namespace ThiTracNghiem_BackEndAPI.ViewModels
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string CorrectAnswers { get; set; }
        public string AnswerExplain { get; set; }
    }
}
