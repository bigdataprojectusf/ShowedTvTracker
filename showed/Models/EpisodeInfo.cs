using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace showed.Models
{
    public class EpisodeInfo
    {
        public int EpisodeInfoId { get; set; }
        public int ThetvdbEpisodeId { get; set; }
        public bool IsWatched { get; set; }
        public int ShowInfoId { get; set; }
    }
}