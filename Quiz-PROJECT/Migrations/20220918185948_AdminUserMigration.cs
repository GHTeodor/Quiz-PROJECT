using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz_PROJECT.Migrations
{
    public partial class AdminUserMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO Users (FirstName, Username ,Email ,Phone ,Password ,ConfirmPassword ,Age ,Role ,CreatedAt ,UpdatedAt) VALUES ('FirstName', 'Username' ,'Email@example.com' ,'+38(098)-76-54-321' ,'SecretAdminPassword' ,'SecretAdminPassword' , NULL ,'ADMIN' ,'2022-09-18T19:13:16.005Z' ,NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM Users WHERE Id = 1");
        }
    }
}
