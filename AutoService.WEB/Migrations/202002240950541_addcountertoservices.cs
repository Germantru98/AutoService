namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addcountertoservices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Counter", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Services", "Counter");
        }
    }
}