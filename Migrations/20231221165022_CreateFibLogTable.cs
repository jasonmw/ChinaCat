using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChinaCatSunflower.Migrations
{
    /// <inheritdoc />
    public partial class CreateFibLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("""
CREATE TABLE public.fib_log (
id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
fib BIGINT NOT NULL,
user_name VARCHAR(255) NULL,
created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
