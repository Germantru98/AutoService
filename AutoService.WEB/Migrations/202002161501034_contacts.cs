namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class contacts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Value = c.String(),
                    isActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.ContactItems");
        }
    }
}