using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using showed.Repositories;
using TVDBSharp;
using TVDBSharp.Models;
using Microsoft.AspNet.Identity;
using showed.Models;
using showed.ViewModels;

namespace showed.Controllers
{
    public class ShowController : Controller
    {
        private TVDB _tvdb;
        private IMembersRepository membersDb;
        private IShowInfoRepository showInfoDb;

        public ShowController()
        {
            _tvdb = new TVDB("86F59A0BFBA75DB4");
            membersDb = new MembersRepository();
            showInfoDb = new ShowInfoRepository();
        }

        
        public ActionResult GetJsonShowsFollows()
        {
            var userId = User.Identity.GetUserId();
            var listOfMembers = membersDb.All.ToList();
            Member member = listOfMembers.Find(c => c.AccountUserId.Contains(userId));
            var allshowinfos = showInfoDb.All.ToList();
            var followedShows = allshowinfos.FindAll(c => c.MemberId.Equals(member.MemberId)).ToList();

            List<CalenderEvent> calEvents = new List<CalenderEvent>();

            foreach (var followed in followedShows)
            {
                var result = _tvdb.GetShow(followed.ShowId);
                var episodeList = result.Episodes;
                int id = 0;
                foreach (var episodes in episodeList)
                {
                    var dateAired = episodes.FirstAired;
                    CalenderEvent cEvent = new CalenderEvent()
                    {
                        allday = "",
                        title = result.Name + " : " + episodes.Title,
                        id = id.ToString(),
                        start = dateAired.GetValueOrDefault().Year + "-" 
                                + dateAired.GetValueOrDefault().Month + "-"
                                + dateAired.GetValueOrDefault().Day
                    };
                    System.Console.WriteLine(cEvent.start);
                    calEvents.Add(cEvent);
                    id++;
                }
            }

            return Json(calEvents, JsonRequestBehavior.AllowGet);
        }

        // GET: Show
        //My shows / manage shows page
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var listOfMembers = membersDb.All.ToList();
            Member member = listOfMembers.Find(c => c.AccountUserId.Contains(userId));
            var allshowinfos = showInfoDb.All.ToList();
            var followedShows = allshowinfos.FindAll(c => c.MemberId.Equals(member.MemberId)).ToList();

            var foundShows = new List<Show>();

            foreach (var showsfollowd in followedShows)
            {
                var result = _tvdb.GetShow(showsfollowd.ShowId);
                foundShows.Add(result);
            }

            ShowIndexViewModel viewModel = new ShowIndexViewModel();
            viewModel.Shows = foundShows;
            viewModel.ShowInfos = followedShows;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UnfollowShow(int showId)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var listOfMembers = membersDb.All.ToList();
                var member = listOfMembers.First(c => c.AccountUserId.Contains(userId));

                var allShowInfos = showInfoDb.All.ToList();
                var followedShows = allShowInfos.FindAll(c => c.MemberId.Equals(member.MemberId)).ToList();
                var showsToRemove = followedShows.FindAll(c => c.ShowId.Equals(showId)).ToList();

                foreach (var show in showsToRemove)
                {
                    showInfoDb.Delete(show);
                    showInfoDb.Save();
                }

                return RedirectToAction("Index");
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult FollowShow(int showId)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var listOfMembers = membersDb.All.ToList();
                var member = listOfMembers.First(c => c.AccountUserId.Contains(userId));

                ShowInfo showInfo = new ShowInfo
                {
                    ShowId = showId, 
                    MemberId = member.MemberId
                };

                showInfoDb.InsertOrUpdate(showInfo);
                showInfoDb.Save();
                 
                return RedirectToAction("Index");
            }
            return View("Error");
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