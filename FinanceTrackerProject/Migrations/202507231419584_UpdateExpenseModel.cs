namespace FinanceTrackerProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateExpenseModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Expenses", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Expenses", "UserId");
            AddForeignKey("dbo.Expenses", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Expenses", new[] { "UserId" });
            DropColumn("dbo.Expenses", "UserId");
            DropColumn("dbo.Expenses", "Date");
        }
    }
}
