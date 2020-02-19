namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class discv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "DiscountId", "dbo.Discounts");
            DropIndex("dbo.Services", new[] { "DiscountId" });
            AddColumn("dbo.Discounts", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Discounts", "ServiceWithDiscountId", c => c.Int(nullable: false));
            AddColumn("dbo.Discounts", "Service_ServiceId", c => c.Int());
            CreateIndex("dbo.Discounts", "Service_ServiceId");
            AddForeignKey("dbo.Discounts", "Service_ServiceId", "dbo.Services", "ServiceId");
            DropColumn("dbo.Discounts", "StartDate");
            DropColumn("dbo.Discounts", "FinishDate");
            DropColumn("dbo.Services", "DiscountId");
        }

        public override void Down()
        {
            AddColumn("dbo.Services", "DiscountId", c => c.Int(nullable: false));
            AddColumn("dbo.Discounts", "FinishDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Discounts", "StartDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Discounts", "Service_ServiceId", "dbo.Services");
            DropIndex("dbo.Discounts", new[] { "Service_ServiceId" });
            DropColumn("dbo.Discounts", "Service_ServiceId");
            DropColumn("dbo.Discounts", "ServiceWithDiscountId");
            DropColumn("dbo.Discounts", "isActive");
            CreateIndex("dbo.Services", "DiscountId");
            AddForeignKey("dbo.Services", "DiscountId", "dbo.Discounts", "DiscountId", cascadeDelete: true);
        }
    }
}