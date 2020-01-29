namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DataMigration1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Cars", new[] { "User_Id" });
            DropColumn("dbo.Cars", "ApplicationUserId");
            RenameColumn(table: "dbo.Cars", name: "User_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.Cars", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Cars", "ApplicationUserId");
        }

        public override void Down()
        {
            DropIndex("dbo.Cars", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Cars", "ApplicationUserId", c => c.Int());
            RenameColumn(table: "dbo.Cars", name: "ApplicationUserId", newName: "User_Id");
            AddColumn("dbo.Cars", "ApplicationUserId", c => c.Int());
            CreateIndex("dbo.Cars", "User_Id");
        }
    }
}