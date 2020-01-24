using AutoService.DAL.EF;
using AutoService.DAL.Entities;
using AutoService.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace AutoService.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ContactDataContext db;
        private ContactDataRepository contactDataRepository;

        public EFUnitOfWork(string connectionString)
        {
            db = new ContactDataContext(connectionString);
        }

        public IRepository<ContactData> ContactDatas
        {
            get
            {
                if (contactDataRepository == null)
                    contactDataRepository = new ContactDataRepository(db);
                return contactDataRepository;
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}