using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.UI.Sqlite
{
    public class Root<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int ItemCount { get; set; }
    }
}