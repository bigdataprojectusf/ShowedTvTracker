using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using showed.Models;

namespace showed.Repositories
{
    public interface IEpisodeInfoRepository
    {
        IQueryable<EpisodeInfo> All { get; }
        IQueryable<ShowInfo> AllShowInfos { get; }
        IQueryable<EpisodeInfo> AllIncluding(params Expression<Func<EpisodeInfo, object>>[] includeProperties);
        EpisodeInfo Find(int? id);
        void InsertOrUpdate(EpisodeInfo episodeInfo);
        void Delete(EpisodeInfo episodeInfo);
        void Save();
        void Dispose();
    }

    public class EpisodeInfoRepository : IEpisodeInfoRepository
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private ShowInfoRepository showDb = new ShowInfoRepository();

        public IQueryable<EpisodeInfo> All
        {
            get { return context.EpisodeInfos; }
        }

        public IQueryable<ShowInfo> AllShowInfos
        {
            get { return showDb.All; }
        }

        public IQueryable<EpisodeInfo> AllIncluding(params Expression<Func<EpisodeInfo, object>>[] includeProperties)
        {
            IQueryable<EpisodeInfo> query = context.EpisodeInfos;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }


        public EpisodeInfo Find(int? id)
        {
            return context.EpisodeInfos.Find(id);
        }

        public void InsertOrUpdate(EpisodeInfo episodeInfo)
        {
            if (episodeInfo.EpisodeInfoId == 0)
            {
                context.EpisodeInfos.Add(episodeInfo);
            }
            else
            {
                context.Entry(episodeInfo).State = EntityState.Modified;
            }
            //how can this method know if the student is new or existing.
            //if studentId == 0 //then it is new, if not it is existing object.
        }

        public void Delete(EpisodeInfo episodeInfo)
        {
            // Student student = Find(id);
            context.Entry(episodeInfo).State = EntityState.Deleted;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}