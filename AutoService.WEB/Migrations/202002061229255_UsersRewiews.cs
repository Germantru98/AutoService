namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersRewiews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRewiews",
                c => new
                    {
                        UserRewiewId = c.Int(nullable: false, identity: true),
                        RewiewText = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserRewiewId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRewiews", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserRewiews", new[] { "ApplicationUserId" });
            DropTable("dbo.UserRewiews");
        }
    }
}
