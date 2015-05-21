using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        private IEpisodeInfoRepository episodeInfoDb;

        public ShowController()
        {
            _tvdb = new TVDB("86F59A0BFBA75DB4");
            membersDb = new MembersRepository();
            showInfoDb = new ShowInfoRepository();
            episodeInfoDb = new EpisodeInfoRepository();
        }

        
        public ActionResult GetJsonShowsFollows()
        {
            var userId = User.Identity.GetUserId();
            //var listOfMembers = membersDb.All.ToList();
            //Member member = listOfMembers.Find(c => c.AccountUserId.Contains(userId));
            Member member = membersDb.All.First(c => c.AccountUserId.Contains(userId));
            //var allshowinfos = showInfoDb.All.ToList();
            //var followedShows = allshowinfos.FindAll(c => c.MemberId.Equals(member.MemberId)).ToList();
            var followedShows = showInfoDb.All.Where(c => c.MemberId.Equals(member.MemberId)).ToList();

            List<CalenderEvent> calEvents = new List<CalenderEvent>();

            foreach (var followed in followedShows)
            {
                var result = _tvdb.GetShow(followed.ShowId);
                var episodeList = result.Episodes;
                int id = 0;
                foreach (var episodes in episodeList)
                {
                    //var allEpisodes = episodeInfoDb.All.ToList();
                    //var watchedEpisodes = allEpisodes.FindAll(c => c.ShowInfoId.Equals(followed.ShowInfoId)).ToList();
                    var watchedEpisodes =
                        episodeInfoDb.All.Where(c => c.ShowInfoId.Equals(followed.ShowInfoId)).ToList();
                    var dateAired = episodes.FirstAired;
                    string watchedEpisodeInfoId = "";
                    string isWatched = "false";
                    string classNameEvent = "eventNotWatched";

                    if (watchedEpisodes.FirstOrDefault(c=> c.ThetvdbEpisodeId.Equals(episodes.Id)) != null)
                    {
                        watchedEpisodeInfoId = watchedEpisodes.First(c => c.ThetvdbEpisodeId.Equals(episodes.Id)).EpisodeInfoId.ToString();
                        isWatched = "true";
                        classNameEvent = "eventWatched";
                    }

                    CalenderEvent cEvent = new CalenderEvent()
                    {
                        allday = "",
                        title = result.Name + "\n" + "(" + episodes.SeasonNumber + "x" + episodes.EpisodeNumber + ")" + episodes.Title,
                        id = id.ToString(),
                        start = dateAired.GetValueOrDefault().Year + "-" 
                                + dateAired.GetValueOrDefault().Month + "-"
                                + dateAired.GetValueOrDefault().Day,
                        className = classNameEvent,
                        completed = isWatched,
                        thetvdbepisodeid = episodes.Id.ToString(),
                        showinfoid = followed.ShowInfoId.ToString(),
                        episodeinfoid = watchedEpisodeInfoId
                    };
                    //System.Console.WriteLine(cEvent.start);
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

            //var listOfMembers = membersDb.All.ToList();
            //Member member = listOfMembers.Find(c => c.AccountUserId.Contains(userId));
            Member member = membersDb.All.First(c => c.AccountUserId.Contains(userId));
            //var allshowinfos = showInfoDb.All.ToList();
            //var followedShows = allshowinfos.FindAll(c => c.MemberId.Equals(member.MemberId)).ToList();
            var followedShows = showInfoDb.All.Where(c => c.MemberId.Equals(member.MemberId)).ToList();
            var foundShows = followedShows.Select(showsfollowd => _tvdb.GetShow(showsfollowd.ShowId)).ToList();

            ShowIndexViewModel viewModel = new ShowIndexViewModel
            {
                Shows = foundShows, 
                ShowInfos = followedShows
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UnfollowShow(int showId)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                //var listOfMembers = membersDb.All.ToList();
                //var member = listOfMembers.First(c => c.AccountUserId.Contains(userId));
                Member member = membersDb.All.First(c => c.AccountUserId.Contains(userId));

                //var allShowInfos = showInfoDb.All.ToList();
                //var followedShows = allShowInfos.FindAll(c => c.MemberId.Equals(member.MemberId)).ToList();
                //var showsToRemove = followedShows.FindAll(c => c.ShowId.Equals(showId)).ToList();
                var showToRemove =
                    showInfoDb.All.Where(c => c.MemberId.Equals(member.MemberId))
                        .First(c => c.ShowId.Equals(showId));
            
                showInfoDb.Delete(showToRemove);
                showInfoDb.Save();
                
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

                //var listOfMembers = membersDb.All.ToList();
                //var member = listOfMembers.First(c => c.AccountUserId.Contains(userId));
                Member member = membersDb.All.First(c => c.AccountUserId.Contains(userId));
                var allreadyFollowsShow =
                    showInfoDb.All.FirstOrDefault(c => c.MemberId.Equals(member.MemberId) && c.ShowId.Equals(showId));
                if (allreadyFollowsShow == null)
                {
                    ShowInfo showInfo = new ShowInfo
                    {
                        ShowId = showId, 
                        MemberId = member.MemberId
                    };

                    showInfoDb.InsertOrUpdate(showInfo);
                    showInfoDb.Save();   
                }
                 
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public ActionResult ShowDetails(int showId)
        {
            var showResult = _tvdb.GetShow(showId);
            var viewModel = new ShowDetailsViewModel
            {
                Show = showResult, 
                EpisodeInfos = null
            };

            return View(viewModel);
        }

        public ActionResult LoggedInShowDetails(int showId, int showInfoId)
        {
            var showResult = _tvdb.GetShow(showId);
            var viewModel = new ShowDetailsViewModel
            {
                Show = showResult
            };
            //var allEpisodes = episodeInfoDb.All.ToList();
            //viewModel.EpisodeInfos = allEpisodes.FindAll(c => c.ShowInfoId.Equals(showInfoId)).ToList();
            viewModel.EpisodeInfos = episodeInfoDb.All.Where(c => c.ShowInfoId.Equals(showInfoId)).ToList();
            var showInfo = showInfoDb.Find(showInfoId);
            viewModel.ShowInfo = showInfo;
            return View("ShowDetails",viewModel);
        }

        public ActionResult WatchEpisode(int theTvDbEpisodeId, int showInfoId)
        {
            if (ModelState.IsValid)
            {
                //var listOfShows = showInfoDb.All.ToList();
                //var showInfo = listOfShows.First(c=> c.ShowInfoId.Equals(showInfoId)); 
                var showInfo = showInfoDb.All.First(c => c.ShowInfoId.Equals(showInfoId));

                EpisodeInfo episodeInfo = new EpisodeInfo
                {
                   ShowInfoId = showInfo.ShowInfoId,
                   IsWatched = true,
                   ThetvdbEpisodeId = theTvDbEpisodeId
                };

                episodeInfoDb.InsertOrUpdate(episodeInfo);
                episodeInfoDb.Save();

                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public ActionResult UnWatchEpisode(int episodeInfoId)
        {
            if (ModelState.IsValid)
            {
                var episodeToDelete = episodeInfoDb.Find(episodeInfoId);
                episodeInfoDb.Delete(episodeToDelete);
                episodeInfoDb.Save();

                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public JsonResult WatchEpisodeJson(int theTvDbEpisodeId, int showInfoId)
        {
            if (ModelState.IsValid)
            {
                //var listOfShows = showInfoDb.All.ToList();
                //var showInfo = listOfShows.First(c => c.ShowInfoId.Equals(showInfoId));
                var showInfo = showInfoDb.All.First(c => c.ShowInfoId.Equals(showInfoId));

                EpisodeInfo episodeInfo = new EpisodeInfo
                {
                    ShowInfoId = showInfo.ShowInfoId,
                    IsWatched = true,
                    ThetvdbEpisodeId = theTvDbEpisodeId
                };

                episodeInfoDb.InsertOrUpdate(episodeInfo);
                episodeInfoDb.Save();

                var stringEpisodeInfo = episodeInfo.EpisodeInfoId.ToString();

                return Json(stringEpisodeInfo);
            }
            return Json(false);
        }

        public JsonResult UnWatchEpisodeJson(int episodeInfoId)
        {
            if (ModelState.IsValid)
            {
                var episodeToDelete = episodeInfoDb.Find(episodeInfoId);
                episodeInfoDb.Delete(episodeToDelete);
                episodeInfoDb.Save();

                return Json(true);
            }
            return Json(false);
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
    }
}