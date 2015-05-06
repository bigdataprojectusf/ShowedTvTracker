using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using showed.Models;
using TVDBSharp.Models;

namespace showed.ViewModels
{
    public class ShowIndexViewModel
    {
        public List<Show> Shows { get; set; }
        public List<ShowInfo> ShowInfos { get; set; }
    }
}