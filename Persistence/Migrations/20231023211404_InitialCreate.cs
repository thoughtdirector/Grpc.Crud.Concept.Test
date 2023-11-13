using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    permissionIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    permissionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    permissionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.permissionIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userPhone = table.Column<int>(type: "int", nullable: false),
                    userPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    permissionspermissionIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rolesroleIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.permissionspermissionIdentifier, x.rolesroleIdentifier });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_permissionspermissionIdentifier",
                        column: x => x.permissionspermissionIdentifier,
                        principalTable: "Permissions",
                        principalColumn: "permissionIdentifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_rolesroleIdentifier",
                        column: x => x.rolesroleIdentifier,
                        principalTable: "Roles",
                        principalColumn: "roleIdentifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    rolesroleIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    usersuserIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.rolesroleIdentifier, x.usersuserIdentifier });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_rolesroleIdentifier",
                        column: x => x.rolesroleIdentifier,
                        principalTable: "Roles",
                        principalColumn: "roleIdentifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_usersuserIdentifier",
                        column: x => x.usersuserIdentifier,
                        principalTable: "Users",
                        principalColumn: "userIdentifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_rolesroleIdentifier",
                table: "PermissionRole",
                column: "rolesroleIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_usersuserIdentifier",
                table: "RoleUser",
                column: "usersuserIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
