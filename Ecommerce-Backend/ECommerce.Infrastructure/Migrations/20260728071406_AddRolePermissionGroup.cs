using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermissionGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RolePermissionGroups",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolePermissionGroupPermissionGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RolePermissionGroupRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissionGroups", x => new { x.RoleId, x.PermissionGroupId });
                    table.ForeignKey(
                        name: "FK_RolePermissionGroups_PermissionGroups_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionGroups_RolePermissionGroups_RolePermissionGroupRoleId_RolePermissionGroupPermissionGroupId",
                        columns: x => new { x.RolePermissionGroupRoleId, x.RolePermissionGroupPermissionGroupId },
                        principalTable: "RolePermissionGroups",
                        principalColumns: new[] { "RoleId", "PermissionGroupId" });
                    table.ForeignKey(
                        name: "FK_RolePermissionGroups_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionGroups_PermissionGroupId",
                table: "RolePermissionGroups",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionGroups_RolePermissionGroupRoleId_RolePermissionGroupPermissionGroupId",
                table: "RolePermissionGroups",
                columns: new[] { "RolePermissionGroupRoleId", "RolePermissionGroupPermissionGroupId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissionGroups");
        }
    }
}
