using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineСinema.Migrations
{
    /// <inheritdoc />
    public partial class mainInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fkepisode624417",
                table: "episode");

            migrationBuilder.DropForeignKey(
                name: "fkseasone718967",
                table: "seasone");

            migrationBuilder.DropForeignKey(
                name: "fktags_title298316",
                table: "tags_title");

            migrationBuilder.DropForeignKey(
                name: "fktags_title691523",
                table: "tags_title");

            migrationBuilder.DropForeignKey(
                name: "fktitle479355",
                table: "title");

            migrationBuilder.DropForeignKey(
                name: "fktitle660657",
                table: "title");

            migrationBuilder.AddForeignKey(
                name: "fkepisode624417",
                table: "episode",
                column: "seasoneid",
                principalTable: "seasone",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fkseasone718967",
                table: "seasone",
                column: "titleid",
                principalTable: "title",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fktags_title298316",
                table: "tags_title",
                column: "titleid",
                principalTable: "title",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fktags_title691523",
                table: "tags_title",
                column: "tagsid",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fktitle479355",
                table: "title",
                column: "coverimageid",
                principalTable: "image",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fktitle660657",
                table: "title",
                column: "tileimageid",
                principalTable: "image",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fkepisode624417",
                table: "episode");

            migrationBuilder.DropForeignKey(
                name: "fkseasone718967",
                table: "seasone");

            migrationBuilder.DropForeignKey(
                name: "fktags_title298316",
                table: "tags_title");

            migrationBuilder.DropForeignKey(
                name: "fktags_title691523",
                table: "tags_title");

            migrationBuilder.DropForeignKey(
                name: "fktitle479355",
                table: "title");

            migrationBuilder.DropForeignKey(
                name: "fktitle660657",
                table: "title");

            migrationBuilder.AddForeignKey(
                name: "fkepisode624417",
                table: "episode",
                column: "seasoneid",
                principalTable: "seasone",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fkseasone718967",
                table: "seasone",
                column: "titleid",
                principalTable: "title",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fktags_title298316",
                table: "tags_title",
                column: "titleid",
                principalTable: "title",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fktags_title691523",
                table: "tags_title",
                column: "tagsid",
                principalTable: "tags",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fktitle479355",
                table: "title",
                column: "coverimageid",
                principalTable: "image",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fktitle660657",
                table: "title",
                column: "tileimageid",
                principalTable: "image",
                principalColumn: "id");
        }
    }
}
