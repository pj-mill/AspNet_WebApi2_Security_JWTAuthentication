namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Identity.People",
                c => new
                    {
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Level = c.Byte(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicationUserId)
                .ForeignKey("Identity.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.FirstName, name: "IDX_PERSON_FIRSTNAME")
                .Index(t => t.LastName, name: "IDX_PERSON_LASTNAME");
            
            CreateTable(
                "Identity.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "Identity.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Identity.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Identity.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Identity.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Identity.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Identity.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Identity.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Identity.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("Identity.AspNetUserRoles", "RoleId", "Identity.AspNetRoles");
            DropForeignKey("Identity.People", "ApplicationUserId", "Identity.AspNetUsers");
            DropForeignKey("Identity.AspNetUserRoles", "UserId", "Identity.AspNetUsers");
            DropForeignKey("Identity.AspNetUserLogins", "UserId", "Identity.AspNetUsers");
            DropForeignKey("Identity.AspNetUserClaims", "UserId", "Identity.AspNetUsers");
            DropIndex("Identity.AspNetRoles", "RoleNameIndex");
            DropIndex("Identity.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("Identity.AspNetUserRoles", new[] { "UserId" });
            DropIndex("Identity.AspNetUserLogins", new[] { "UserId" });
            DropIndex("Identity.AspNetUserClaims", new[] { "UserId" });
            DropIndex("Identity.AspNetUsers", "UserNameIndex");
            DropIndex("Identity.People", "IDX_PERSON_LASTNAME");
            DropIndex("Identity.People", "IDX_PERSON_FIRSTNAME");
            DropIndex("Identity.People", new[] { "ApplicationUserId" });
            DropTable("Identity.AspNetRoles");
            DropTable("Identity.AspNetUserRoles");
            DropTable("Identity.AspNetUserLogins");
            DropTable("Identity.AspNetUserClaims");
            DropTable("Identity.AspNetUsers");
            DropTable("Identity.People");
        }
    }
}
