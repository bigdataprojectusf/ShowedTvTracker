using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace showed.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public ICollection<int> ShowsIdCollection { get; set; }
    }
}