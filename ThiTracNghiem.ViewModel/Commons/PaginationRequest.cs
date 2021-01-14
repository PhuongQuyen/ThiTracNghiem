using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Commons
{
   public class PaginationRequest
    {
        public PaginationRequest(
            int totalItems,
            int currentPage = 1,
            int pageSize = 10,
            int maxPages = 10)
        {
            // calculate total pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            // ensure current page isn't out of range
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            else if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            int startPage, endPage;
            if (totalPages <= maxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                var maxPagesBeforeCurrentPage = (int)Math.Floor((decimal)maxPages / (decimal)2);
                var maxPagesAfterCurrentPage = (int)Math.Ceiling((decimal)maxPages / (decimal)2) - 1;
                if (currentPage <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (currentPage + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = currentPage - maxPagesBeforeCurrentPage;
                    endPage = currentPage + maxPagesAfterCurrentPage;
                }
            }

            // calculate start and end item indexes
            var startIndex = (currentPage - 1) * pageSize+1;
            var endIndex = Math.Min(startIndex + pageSize , totalItems - 1);

            // create an array of pages that can be looped over
            //var pages = data.Range(startPage, (endPage + 1) - startPage);

            // update object instance with all pager properties required by the view
            Total = totalItems;
            CurrentPage = currentPage;
            PerPage = pageSize;
            //TotalPages = totalPages;
            FirstPageUrl = startPage+"";
            LastPage = endPage;
            From = startIndex;
            To= endIndex;
            //Data = pages;
        }

        public int CurrentPage { set; get; }
        public List<QuestionAndAnswer> Data { get; set; }
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
