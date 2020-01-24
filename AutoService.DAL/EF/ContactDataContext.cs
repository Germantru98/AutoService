using AutoService.DAL.Entities;
using System.Data.Entity;

namespace AutoService.DAL.EF
{
    public class ContactDataContext : DbContext
    {
        public DbSet<ContactData> ContactDatas { get; set; }

        public ContactDataContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}