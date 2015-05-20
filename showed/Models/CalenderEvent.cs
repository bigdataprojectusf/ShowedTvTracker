using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace showed.Models
{
    public class CalenderEvent
    {
        public string allday { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public string end { get; set; }
        public string start { get; set; }
        public string className { get; set; }
        public string completed { get; set; }
        public string thetvdbepisodeid { get; set; }
        public string showinfoid { get; set; }
        public string episodeinfoid { get; set; }
    }
}