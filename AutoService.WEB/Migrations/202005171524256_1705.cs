namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1705 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Services", "Counter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Counter", c => c.Int(nullable: false));
        }
    }
}
