namespace Skybook.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPlanetImageCapability : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Planets", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Planets", "Image");
        }
    }
}
