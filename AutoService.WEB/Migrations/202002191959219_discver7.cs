namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class discver7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Discounts", "Service_ServiceId", "dbo.Services");
            DropIndex("dbo.Discounts", new[] { "Service_ServiceId" });
            AddColumn("dbo.Services", "DiscountId", c => c.Int());
            CreateIndex("dbo.Services", "DiscountId");
            AddForeignKey("dbo.Services", "DiscountId", "dbo.Discounts", "DiscountId");
            DropColumn("dbo.Discounts", "Service_ServiceId");
        }

        public override void Down()
        {
            AddColumn("dbo.Discounts", "Service_ServiceId", c => c.Int());
            DropForeignKey("dbo.Services", "DiscountId", "dbo.Discounts");
            DropIndex("dbo.Services", new[] { "DiscountId" });
            DropColumn("dbo.Services", "DiscountId");
            CreateIndex("dbo.Discounts", "Service_ServiceId");
            AddForeignKey("dbo.Discounts", "Service_ServiceId", "dbo.Services", "ServiceId");
        }
    }
}