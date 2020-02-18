namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServieImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "ServiceImageHref", c => c.String(nullable: false));
            DropColumn("dbo.Services", "ImageData");
            DropColumn("dbo.Services", "ImageMimeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "ImageMimeType", c => c.String());
            AddColumn("dbo.Services", "ImageData", c => c.Binary());
            DropColumn("dbo.Services", "ServiceImageHref");
        }
    }
}
