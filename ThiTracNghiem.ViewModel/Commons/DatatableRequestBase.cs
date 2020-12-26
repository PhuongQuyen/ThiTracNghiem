
using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Commons
{
    public class DatatableRequestBase
    {
        public int Skip { set; get; }
        public string Draw { set; get; }
        public int PageSize { set; get; }
        public string searchValue { set; get; }
        public string sortColumnDirection { set; get; }
        public string sortColumn { set; get; }
    }
}
