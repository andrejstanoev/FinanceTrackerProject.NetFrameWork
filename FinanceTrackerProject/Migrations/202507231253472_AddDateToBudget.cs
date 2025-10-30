namespace FinanceTrackerProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateToBudget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Budgets", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Budgets", "Date");
        }
    }
}
