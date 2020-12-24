using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Questiondetails
    {
        public int QuestionId { get; set; }
        public int ExamId { get; set; }

        public virtual Exams Exam { get; set; }
        public virtual Questions Question { get; set; }
    }
}
