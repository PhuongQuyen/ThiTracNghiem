using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Joinroom
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public double? Score { get; set; }
        public DateTime? TimeSubmitExam { get; set; }
    }
}
