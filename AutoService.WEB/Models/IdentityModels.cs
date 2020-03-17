using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AutoService.WEB.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Car> Cars { get; set; }
        public ICollection<UserRewiew> UserRewiews { get; set; }
        public ICollection<BasketItem> Basket { get; set; }
        public string RealName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        public ApplicationUser()
            : base()
        {
            Cars = new List<Car>();
            UserRewiews = new List<UserRewiew>();
            Basket = new List<BasketItem>();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<UserRewiew> UserRewiews { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<JobVacancy> JobVacancies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ContactItem> Contacts { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<HomeMainCarouselItem> HomeMainCarouselItems { get; set; }
        public DbSet<ServicesSummary> ServicesSummaries { get; set; }
    }
}