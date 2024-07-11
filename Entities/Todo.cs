using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities
{
    public class Todo
    {
        
        public int id { get; set; }
        public string name { get; set; }
        public bool urgent { get; set; } = false;
        public bool done { get; set; } = false;
    }
}
