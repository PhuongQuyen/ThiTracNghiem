using System;
using System.Collections.Generic;
using System.Text;

namespace ThiTracNghiem_ViewModel.Commons
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }
    }
}
