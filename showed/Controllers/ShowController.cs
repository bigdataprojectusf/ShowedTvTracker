using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TVDBSharp;
using TVDBSharp.Models;

namespace showed.Controllers
{
    public class ShowController : Controller
    {
        private TVDB _tvdb;

        public ShowController()
        {
            _tvdb = new TVDB("86F59A0BFBA75DB4");
        }

        // GET: Show
        //My shows / manage shows page
        public ActionResult Index()
        {
            return View();
        }

        //Calender view page
        public ActionResult MyCalender()
        {
            return View();
        }

        //Page returning searchresults
        [HttpGet]
        public ActionResult SearchResults()
        {
            ViewBag.SearchTerm = "";
            return View();
        }

        [HttpPost]
        public ActionResult SearchResults(string search)
        {
            var results = _tvdb.Search(search, 3);
            IEnumerable<Show> shows = results;
            return View(shows);
        }

        public ActionResult Follow()
        {
            return View();
        }
    }
}