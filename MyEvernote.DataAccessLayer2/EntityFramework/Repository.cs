﻿using MyEvernote.Common;
using MyEvernote.Entities2;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using MyEvernote.Core.DataAccess;

namespace MyEvernote.DataAccessLayer2.EntityFramework
{
    public class Repository<T> :RepositoryBase, IDataAccess<T> where T:class
    {
       
        private DbSet<T> _objectSet;
        public Repository()
        {
            
            _objectSet = context.Set<T>();
        }
        public List<T> List()
        {
            return _objectSet.ToList(); 
        }
        public IQueryable<T> ListIQueryable()
        {
            //Iqueryable döner bu methodu ne zaman çağarırsak o zman SQL egider
            //detaylı araştır Iqueryable konusuu
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T,bool>>where)
        {
            return _objectSet.Where(where).ToList();    
        }
        public int Insert(T obj)
        {

            _objectSet.Add(obj);

            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;
                // aşşağıdaki özellikler Myettitybaseden alınacağı için 
                //her defasında yeniden yazılmasına gerek yok obj den türetip aşşağıda aldık
                o.CretedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            return Save();
        }
        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now; ;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }
            return Save();
        }
        public int Delete(T obj)
        {
          
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);

        }

    }
}
