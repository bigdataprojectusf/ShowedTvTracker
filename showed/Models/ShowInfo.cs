using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace showed.Models
{
    public class ShowInfo
    {
        public int ShowInfoId { get; set; }
        public int ShowId { get; set; }
        //public ICollection<EpisodeInfo> EpisodeInfos { get; set; }
        public virtual Member Member { get; set; }
        public int MemberId { get; set; }
    }
}