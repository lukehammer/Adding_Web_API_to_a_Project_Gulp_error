﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackerShared.Data
{
    public abstract class BaseRepository<TEntity>
        where TEntity : class
    {
        protected Context Context { get; private set; }

        public BaseRepository(Context context)
        {
            Context = context;
        }

        public abstract TEntity Get(int id, bool includeRelatedEntities = true);
        public abstract IList<TEntity> GetList();

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var set = Context.Set<TEntity>();
            var entity = set.Find(id);
            set.Remove(entity);
            Context.SaveChanges();
        }
    }
}
