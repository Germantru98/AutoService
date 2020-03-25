namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasketItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ServiceId = c.Int(nullable: false),
                    UserId = c.String(),
                    ApplicationUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);

            CreateTable(
                "dbo.CarBrands",
                c => new
                {
                    BrandId = c.Int(nullable: false, identity: true),
                    BrandName = c.String(),
                    ImageHref = c.String(),
                })
                .PrimaryKey(t => t.BrandId);

            CreateTable(
                "dbo.Cars",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Model = c.String(),
                    Color = c.String(),
                    Year = c.String(),
                    ApplicationUserId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    RealName = c.String(),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.ContactItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Value = c.String(),
                    isActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Discounts",
                c => new
                {
                    DiscountId = c.Int(nullable: false, identity: true),
                    Value = c.Int(nullable: false),
                    StartDate = c.DateTime(nullable: false),
                    FinishDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.DiscountId);

            CreateTable(
                "dbo.HomeMainCarouselItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    ImageHref = c.String(),
                    RouteHref = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.JobVacancies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    VacancyName = c.String(nullable: false),
                    VacancyDescription = c.String(nullable: false),
                    Salary = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    ContactPhone = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.Services",
                c => new
                {
                    ServiceId = c.Int(nullable: false, identity: true),
                    ServiceName = c.String(),
                    Price = c.Int(nullable: false),
                    ServiceImageHref = c.String(),
                    DiscountId = c.Int(),
                    Counter = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ServiceId)
                .ForeignKey("dbo.Discounts", t => t.DiscountId)
                .Index(t => t.DiscountId);

            CreateTable(
                "dbo.ServicesSummaries",
                c => new
                {
                    SummaryId = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    UserCarId = c.Int(),
                    ServiceList = c.String(),
                    DateOfCreating = c.DateTime(nullable: false),
                    DayOfWork = c.DateTime(nullable: false),
                    TotalPrice = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.SummaryId);

            CreateTable(
                "dbo.UserReviews",
                c => new
                {
                    UserReviewId = c.Int(nullable: false, identity: true),
                    ReviewText = c.String(),
                    DateOfCreation = c.DateTime(nullable: false),
                    OwnerId = c.String(),
                })
                .PrimaryKey(t => t.UserReviewId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Services", "DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Cars", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BasketItems", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Services", new[] { "DiscountId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Cars", new[] { "ApplicationUserId" });
            DropIndex("dbo.BasketItems", new[] { "ApplicationUser_Id" });
            DropTable("dbo.UserReviews");
            DropTable("dbo.ServicesSummaries");
            DropTable("dbo.Services");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.JobVacancies");
            DropTable("dbo.HomeMainCarouselItems");
            DropTable("dbo.Discounts");
            DropTable("dbo.ContactItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Cars");
            DropTable("dbo.CarBrands");
            DropTable("dbo.BasketItems");
        }
    }
}