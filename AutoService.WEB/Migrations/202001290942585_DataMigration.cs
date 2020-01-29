namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Model = c.String(),
                    Color = c.String(),
                    Year = c.DateTime(nullable: false),
                    ApplicationUserId = c.Int(),
                    User_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Cars", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Cars", new[] { "User_Id" });
            DropTable("dbo.Cars");
        }
    }
}