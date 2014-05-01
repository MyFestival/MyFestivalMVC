namespace MyFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Festival",
                c => new
                    {
                        FestivalId = c.Int(nullable: false, identity: true),
                        FestivalName = c.String(nullable: false, maxLength: 100),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        FestivalLogo = c.String(),
                        Description = c.String(maxLength: 200),
                        UserID = c.Int(nullable: false),
                        FestivalCounty_ID = c.Int(nullable: false),
                        FestivalTown_ID = c.Int(nullable: false),
                        FType_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FestivalId)
                .ForeignKey("dbo.Counties", t => t.FestivalCounty_ID, cascadeDelete: true)
                .ForeignKey("dbo.Towns", t => t.FestivalTown_ID, cascadeDelete: true)
                .ForeignKey("dbo.FestivalTypes", t => t.FType_ID, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserID, cascadeDelete: true)
                .Index(t => t.FestivalCounty_ID)
                .Index(t => t.FestivalTown_ID)
                .Index(t => t.FType_ID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.County",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Town",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FestivalType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FType = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FestivalID = c.Int(nullable: false),
                        EventsName = c.String(nullable: false, maxLength: 100),
                        EventsDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Location = c.String(nullable: false),
                        EventLogo = c.String(),
                        EType_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EventType", t => t.EType_ID, cascadeDelete: true)
                .ForeignKey("dbo.Festival", t => t.FestivalID, cascadeDelete: true)
                .Index(t => t.EType_ID)
                .Index(t => t.FestivalID);
            
            CreateTable(
                "dbo.EventType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EType = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Events", new[] { "FestivalID" });
            DropIndex("dbo.Events", new[] { "EType_ID" });
            DropIndex("dbo.Festivals", new[] { "UserID" });
            DropIndex("dbo.Festivals", new[] { "FType_ID" });
            DropIndex("dbo.Festivals", new[] { "FestivalTown_ID" });
            DropIndex("dbo.Festivals", new[] { "FestivalCounty_ID" });
            DropForeignKey("dbo.Events", "FestivalID", "dbo.Festivals");
            DropForeignKey("dbo.Events", "EType_ID", "dbo.EventTypes");
            DropForeignKey("dbo.Festivals", "UserID", "dbo.UserProfile");
            DropForeignKey("dbo.Festivals", "FType_ID", "dbo.FestivalTypes");
            DropForeignKey("dbo.Festivals", "FestivalTown_ID", "dbo.Towns");
            DropForeignKey("dbo.Festivals", "FestivalCounty_ID", "dbo.Counties");
            DropTable("dbo.EventTypes");
            DropTable("dbo.Events");
            DropTable("dbo.FestivalTypes");
            DropTable("dbo.Towns");
            DropTable("dbo.Counties");
            DropTable("dbo.Festivals");
            DropTable("dbo.UserProfile");
        }
    }
}
