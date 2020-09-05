namespace Moslah.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuickSearches",
                c => new
                    {
                        Source = c.String(nullable: false, maxLength: 128),
                        Destination = c.String(nullable: false, maxLength: 128),
                        RoadDesc = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Source, t.Destination, t.RoadDesc });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.QuickSearches");
        }
    }
}
