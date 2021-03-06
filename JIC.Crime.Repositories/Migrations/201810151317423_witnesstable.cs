namespace JIC.Crime.Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class witnesstable : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.Cases_CaseWitnesses", "WitnessDocument");
            //DropColumn("dbo.Cases_CaseWitnesses", "FileDataDocument");


            CreateTable(
               "dbo.Cases_WitnessSessionLog",
               c => new
               {
                   WitnessID = c.Long(nullable: false),
                   CaseID = c.Int(nullable: false),
                   SessionID = c.Int(nullable: false),
                   PresenceStatus = c.Int(nullable: false),
                   WitnessTestimony = c.String(nullable: false),
               })
               .PrimaryKey(t => t.WitnessID)
               .ForeignKey("dbo.Cases_CaseWitnesses", t => t.WitnessID, cascadeDelete: true)
               .Index(t => t.WitnessID);
        }

        public override void Down()
        {
            AddColumn("dbo.Cases_CaseWitnesses", "FileDataDocument", c => c.Binary());
            AddColumn("dbo.Cases_CaseWitnesses", "WitnessDocument", c => c.String());
        }
    }
}
