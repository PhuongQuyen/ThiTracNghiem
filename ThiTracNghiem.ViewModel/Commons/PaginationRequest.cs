using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Commons
{
   public class PaginationRequest
    {
        public PaginationRequest()
        {
        }

        public int CurrentPage { set; get; }
        private List<QuestionAndAnswer> Data { set; get; }
        public string FirstPageUrl { set; get; }
        public int From { set; get; }
        public int LastPage { set; get; }
        public string LastPageUrl { set; get; }
        public string NextPageUrl { set; get; }
        public string Path { set; get; }
        public int PerPage { set; get; }
        public string PrevPageUrl { set; get; }
        public int To { set; get; }
        public int Total { set; get; }
    }
}
