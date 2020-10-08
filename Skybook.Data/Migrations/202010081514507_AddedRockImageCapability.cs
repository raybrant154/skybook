namespace Skybook.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRockImageCapability : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rocks", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rocks", "Image");
        }
    }
}
