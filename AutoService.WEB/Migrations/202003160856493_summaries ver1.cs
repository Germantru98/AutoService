namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class summariesver1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServicesSummaries",
                c => new
                    {
                        SummaryId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        UserCarId = c.Int(),
                        ServiceList = c.String(),
                        DateOfCreating = c.DateTime(nullable: false),
                        DayOfWork = c.DateTime(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SummaryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServicesSummaries");
        }
    }
}
