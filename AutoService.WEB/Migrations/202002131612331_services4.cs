namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class services4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "ImageData", c => c.Binary());
            AddColumn("dbo.Services", "ImageMimeType", c => c.String());
            DropColumn("dbo.Services", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Image", c => c.Binary());
            DropColumn("dbo.Services", "ImageMimeType");
            DropColumn("dbo.Services", "ImageData");
        }
    }
}
