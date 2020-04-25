namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newCarStorage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BasketItems", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.BasketItems", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.CarsStorageItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId)
                .Index(t => t.CarId);
            
            DropColumn("dbo.BasketItems", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BasketItems", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.CarsStorageItems", "CarId", "dbo.Cars");
            DropIndex("dbo.CarsStorageItems", new[] { "CarId" });
            DropTable("dbo.CarsStorageItems");
            CreateIndex("dbo.BasketItems", "ApplicationUser_Id");
            AddForeignKey("dbo.BasketItems", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
