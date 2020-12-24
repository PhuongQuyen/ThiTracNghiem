using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Exams
    {
        public Exams()
        {
            Questiondetails = new HashSet<Questiondetails>();
            Scores = new HashSet<Scores>();
        }

        public int Id { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDescription { get; set; }
        public int? ExamtypeId { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeLimit { get; set; }
        public string ExamSlug { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }

        public virtual Examtypes Examtype { get; set; }
        public virtual ICollection<Questiondetails> Questiondetails { get; set; }
        public virtual ICollection<Scores> Scores { get; set; }
    }
}
