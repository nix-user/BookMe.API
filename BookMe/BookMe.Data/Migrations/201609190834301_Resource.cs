namespace BookMe.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Resource : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        HasTv = c.Boolean(nullable: false),
                        HasPolycom = c.Boolean(nullable: false),
                        RoomSize = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Resources");
        }
    }
}
