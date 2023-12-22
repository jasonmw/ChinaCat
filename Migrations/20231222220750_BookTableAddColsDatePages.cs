using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChinaCatSunflower.Migrations
{
    /// <inheritdoc />
    public partial class BookTableAddColsDatePages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"
ALTER TABLE public.books
ADD COLUMN published_date date;
ALTER TABLE public.books
ADD COLUMN page_count integer NOT NULL DEFAULT 0;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
