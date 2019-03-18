namespace bugtracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modifications : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Name", c => c.String(maxLength: 30));
            AlterColumn("dbo.Projects", "Description", c => c.String(maxLength: 40));
            AlterColumn("dbo.Tickets", "Title", c => c.String(maxLength: 30));
            AlterColumn("dbo.Tickets", "Description", c => c.String(maxLength: 60));
            AlterColumn("dbo.TicketAttachments", "Description", c => c.String(maxLength: 40));
            AlterColumn("dbo.TicketComments", "CommentTitle", c => c.String(maxLength: 20));
            AlterColumn("dbo.TicketComments", "CommentBody", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TicketComments", "CommentBody", c => c.String());
            AlterColumn("dbo.TicketComments", "CommentTitle", c => c.String());
            AlterColumn("dbo.TicketAttachments", "Description", c => c.String());
            AlterColumn("dbo.Tickets", "Description", c => c.String());
            AlterColumn("dbo.Tickets", "Title", c => c.String());
            AlterColumn("dbo.Projects", "Description", c => c.String());
            AlterColumn("dbo.Projects", "Name", c => c.String());
        }
    }
}
