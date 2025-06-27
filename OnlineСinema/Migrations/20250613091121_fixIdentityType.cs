using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineСinema.Migrations
{
    /// <inheritdoc />
    public partial class fixIdentityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tags_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TagTitle",
                columns: table => new
                {
                    Tagsid = table.Column<Guid>(type: "uuid", nullable: false),
                    Titleid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTitle", x => new { x.Tagsid, x.Titleid });
                });

            migrationBuilder.CreateTable(
                name: "title",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    isfilm = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    coverimageid = table.Column<Guid>(type: "uuid", nullable: false),
                    tileimageid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("title_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fktitle479355",
                        column: x => x.coverimageid,
                        principalTable: "image",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fktitle660657",
                        column: x => x.tileimageid,
                        principalTable: "image",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "seasone",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    orderindex = table.Column<int>(type: "integer", nullable: false),
                    titleid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seasone_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fkseasone718967",
                        column: x => x.titleid,
                        principalTable: "title",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tags_title",
                columns: table => new
                {
                    tagsid = table.Column<Guid>(type: "uuid", nullable: false),
                    titleid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tags_title_pkey", x => new { x.tagsid, x.titleid });
                    table.ForeignKey(
                        name: "fktags_title298316",
                        column: x => x.titleid,
                        principalTable: "title",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fktags_title691523",
                        column: x => x.tagsid,
                        principalTable: "tags",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "episode",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    orderindex = table.Column<int>(type: "integer", nullable: false),
                    seasoneid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("episode_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fkepisode624417",
                        column: x => x.seasoneid,
                        principalTable: "seasone",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_seen",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<string>(type: "text", nullable: false),
                    titleid = table.Column<Guid>(type: "uuid", nullable: true),
                    episodeid = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_seen_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_seen_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fkuserseen149973",
                        column: x => x.episodeid,
                        principalTable: "episode",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fkuserseen355288",
                        column: x => x.titleid,
                        principalTable: "title",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_episode_seasoneid",
                table: "episode",
                column: "seasoneid");

            migrationBuilder.CreateIndex(
                name: "IX_seasone_titleid",
                table: "seasone",
                column: "titleid");

            migrationBuilder.CreateIndex(
                name: "tags_name_key",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_title_titleid",
                table: "tags_title",
                column: "titleid");

            migrationBuilder.CreateIndex(
                name: "IX_title_coverimageid",
                table: "title",
                column: "coverimageid");

            migrationBuilder.CreateIndex(
                name: "IX_title_tileimageid",
                table: "title",
                column: "tileimageid");

            migrationBuilder.CreateIndex(
                name: "title_name_key",
                table: "title",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_seen_episodeid",
                table: "user_seen",
                column: "episodeid");

            migrationBuilder.CreateIndex(
                name: "IX_user_seen_titleid",
                table: "user_seen",
                column: "titleid");

            migrationBuilder.CreateIndex(
                name: "IX_user_seen_UserId",
                table: "user_seen",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tags_title");

            migrationBuilder.DropTable(
                name: "TagTitle");

            migrationBuilder.DropTable(
                name: "user_seen");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "episode");

            migrationBuilder.DropTable(
                name: "seasone");

            migrationBuilder.DropTable(
                name: "title");

            migrationBuilder.DropTable(
                name: "image");
        }
    }
}
