using AutoService.DAL.EF;
using AutoService.DAL.Entities;
using AutoService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AutoService.DAL.Repositories
{
    public class ContactDataRepository : IRepository<ContactData>
    {
        private ContactDataContext db;

        public ContactDataRepository(ContactDataContext context)
        {
            db = context;
        }

        public void Create(ContactData item)
        {
            db.ContactDatas.Add(item);
        }

        public void Delete(int id)
        {
            ContactData cd = db.ContactDatas.Find(id);
            if (cd != null)
                db.ContactDatas.Remove(cd);
        }

        public IEnumerable<ContactData> Find(Func<ContactData, bool> predicate)
        {
            return db.ContactDatas.Where(predicate).ToList();
        }

        public ContactData Get(int id)
        {
            return db.ContactDatas.Find(id);
        }

        public IEnumerable<ContactData> GetAll()
        {
            return db.ContactDatas;
        }

        public void Update(ContactData item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}