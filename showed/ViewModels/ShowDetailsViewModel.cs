using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using showed.Models;
using TVDBSharp.Models;

namespace showed.ViewModels
{
    public class ShowDetailsViewModel
    {
        public Show Show { get; set; }
        public ShowInfo ShowInfo { get; set; }
        public ICollection<EpisodeInfo> EpisodeInfos { get; set; }
    }
}