using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChinaCatSunflower.Migrations
{
    /// <inheritdoc />
    public partial class BookTableUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"
ALTER TABLE public.books
ADD CONSTRAINT uniq_isbn UNIQUE (isbn);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
