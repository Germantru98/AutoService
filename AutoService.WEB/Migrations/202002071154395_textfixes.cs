namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class textfixes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserRewiews", "RewiewText", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserRewiews", "RewiewText", c => c.String());
        }
    }
}
