using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TVDBSharp;
using TVDBSharp.Models;

namespace showed.Controllers
{

    [RequireHttps]
    public class HomeController : Controller
    {
        private TVDB _tvdb;

        public HomeController()
        {
            _tvdb = new TVDB("86F59A0BFBA75DB4");
        }

        public ActionResult Index()
        {
            var topShowsList = new List<Show>
            {
                _tvdb.GetShow(121361),
                _tvdb.GetShow(281662),
                _tvdb.GetShow(257655),
                _tvdb.GetShow(73762),
                _tvdb.GetShow(279121),
                _tvdb.GetShow(260449),
                _tvdb.GetShow(263365),
                _tvdb.GetShow(153021),
                _tvdb.GetShow(248835),
                _tvdb.GetShow(274431),
                _tvdb.GetShow(80379),
                _tvdb.GetShow(270408)
            };
            return View(topShowsList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}