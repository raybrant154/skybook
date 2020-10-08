namespace Skybook.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingImagesCapabilityForAnimals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Animals", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Animals", "Image");
        }
    }
}
