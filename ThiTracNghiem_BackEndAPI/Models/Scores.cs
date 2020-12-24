using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Scores
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public float Score { get; set; }
        public DateTime Date { get; set; }

        public virtual Exams Exam { get; set; }
        public virtual Users User { get; set; }
    }
}
