namespace Skybook.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPlantImageCapability : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plants", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plants", "Image");
        }
    }
}
