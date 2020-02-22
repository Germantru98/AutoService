namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discver8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discounts", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Discounts", "FinishDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Discounts", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Discounts", "isActive");
            DropColumn("dbo.Discounts", "FinishDate");
            DropColumn("dbo.Discounts", "StartDate");
        }
    }
}
