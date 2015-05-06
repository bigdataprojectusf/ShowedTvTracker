using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace showed.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string AccountUserId { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public ICollection<ShowInfo> ShowInfos { get; set; }
    }
}