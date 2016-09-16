namespace BookMe.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Profile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 128),
                        Floor = c.Int(nullable: false),
                        FavouriteRoom = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserProfiles", "UserNameIndex");
            DropTable("dbo.UserProfiles");
        }
    }
}
