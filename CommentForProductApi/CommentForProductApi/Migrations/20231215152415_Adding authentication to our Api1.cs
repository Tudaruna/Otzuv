using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommentForProductApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingauthenticationtoourApi1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    JwtId = table.Column<string>(type: "text", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("type_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    id_role = table.Column<int>(type: "integer", nullable: true),
                    IdRefreshToken = table.Column<int>(type: "integer", nullable: true),
                    IdUser = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_RefreshTokens_IdUser",
                        column: x => x.IdUser,
                        principalTable: "RefreshTokens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "user_id_role_fkey",
                        column: x => x.id_role,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    id_type = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_pkey", x => x.id);
                    table.ForeignKey(
                        name: "product_id_type_fkey",
                        column: x => x.id_type,
                        principalTable: "type",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    score = table.Column<int>(type: "integer", nullable: true),
                    id_user = table.Column<int>(type: "integer", nullable: true),
                    id_product = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("comment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "comment_id_product_fkey",
                        column: x => x.id_product,
                        principalTable: "product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "comment_id_user_fkey",
                        column: x => x.id_user,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_comment_id_product",
                table: "comment",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_comment_id_user",
                table: "comment",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_product_id_type",
                table: "product",
                column: "id_type");

            migrationBuilder.CreateIndex(
                name: "IX_user_IdUser",
                table: "user",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_user_id_role",
                table: "user",
                column: "id_role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "type");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
