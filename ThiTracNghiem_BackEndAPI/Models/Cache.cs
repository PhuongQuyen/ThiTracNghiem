using System;
using System.Collections.Generic;

namespace ThiTracNghiem_BackEndAPI.Models
{
    public partial class Cache
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Expiration { get; set; }
    }
}
