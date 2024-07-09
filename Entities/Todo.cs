using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Todo
    {
        public int id { get; set; }
        public string nama { get; set; }
        public bool urgent { get; set; }
        public bool done { get; set; }
    }
}
