using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut3.Models
{
    public class Enrollment
    {
        public int IdSemester { get; set; }
        public string StartDate { get; set; }
        public Studies study { get; set; }
    }
}
