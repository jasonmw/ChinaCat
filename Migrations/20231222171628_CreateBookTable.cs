using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChinaCatSunflower.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql(@"
CREATE TABLE public.books (
id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
isbn VARCHAR(50) NOT NULL,
title VARCHAR(255) NOT NULL,
author VARCHAR(255) NOT NULL,
image_url VARCHAR(1024) NOT NULL,
book_json JSONB NOT NULL,
created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
