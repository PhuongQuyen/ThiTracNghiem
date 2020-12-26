using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Commons
{
    public class DatatableResult<T>
    {
        public string Draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public T Data { get; set; }
    }
}
