using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using showed.Models;


namespace showed.Repositories
{
    public interface IShowInfoRepository
    {
        IQueryable<ShowInfo> All { get; }
        IQueryable<Member> AllMembers { get; }
        IQueryable<ShowInfo> AllIncluding(params Expression<Func<ShowInfo, object>>[] includeProperties);
        ShowInfo Find(int? id);
        void InsertOrUpdate(ShowInfo showInfo);
        void Delete(ShowInfo showInfo);
        void Save();
        void Dispose();
    }
    
    public class ShowInfoRepository : IShowInfoRepository
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private MembersRepository memberDb = new MembersRepository();

        public IQueryable<ShowInfo> All
        {
            get { return context.ShowInfos; }
        }

        public IQueryable<Member> AllMembers
        {
            get { return memberDb.All; }
        }

        public IQueryable<ShowInfo> AllIncluding(params Expression<Func<ShowInfo, object>>[] includeProperties)
        {
            IQueryable<ShowInfo> query = context.ShowInfos;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }


        public ShowInfo Find(int? id)
        {
            return context.ShowInfos.Find(id);
        }

        public void InsertOrUpdate(ShowInfo showInfo)
        {
            if (showInfo.ShowInfoId == 0)
            {
                context.ShowInfos.Add(showInfo);
            }
            else
            {
                context.Entry(showInfo).State = EntityState.Modified;
            }
            //how can this method know if the student is new or existing.
            //if studentId == 0 //then it is new, if not it is existing object.
        }

        public void Delete(ShowInfo showInfo)
        {
            // Student student = Find(id);
            context.Entry(showInfo).State = EntityState.Deleted;
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