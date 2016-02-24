using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using showed.Models;

namespace showed.Repositories
{
    public interface IMembersRepository
    {
        IQueryable<Member> All { get; }
        IQueryable<Member> AllIncluding(params Expression<Func<Member, object>>[] includeProperties);
        Member Find(int? id);
        void InsertOrUpdate(Member member);
        void Delete(Member member);
        void Save();
        void Dispose();
    }

    public class MembersRepository : IMembersRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public IQueryable<Member> All
        {
            get { return context.Members; }
        }

        public IQueryable<Member> AllIncluding(params Expression<Func<Member, object>>[] includeProperties)
        {
            IQueryable<Member> query = context.Members;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public void Delete(Member member)
        {
            context.Entry(member).State = EntityState.Deleted;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Member Find(int? id)
        {
            return context.Members.Find(id);
        }

        public void InsertOrUpdate(Member member)
        {
            if (member.MemberId == 0)
            {
                context.Members.Add(member);
            }
            else
            {
                context.Entry(member).State = EntityState.Modified;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}