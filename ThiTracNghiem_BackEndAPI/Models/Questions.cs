using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Questions
    {
        public Questions()
        {
            Answers = new HashSet<Answers>();
            Questiondetails = new HashSet<Questiondetails>();
        }

        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public int? ModuleId { get; set; }
        public int QuestionType { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Modules Module { get; set; }
        public virtual ICollection<Answers> Answers { get; set; }
        public virtual ICollection<Questiondetails> Questiondetails { get; set; }
    }
}
