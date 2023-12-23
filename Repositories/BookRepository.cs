using ChinaCatSunflower.Data.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;
using NpgsqlTypes;

namespace ChinaCatSunflower.Repositories;

public class BookRepository : BasePgRepository
{
    public ILogger<BookRepository> Logger { get; }

    public BookRepository(IConfiguration configuration, ILogger<BookRepository> logger) : base(configuration, logger) {
        Logger = logger;
    }
    public async Task<int> AddBook(Book book) {
        await using var conn = await GetNewOpenConnection();
        // add book to database
        var sql = @"
            INSERT INTO public.books (isbn, title, author, image_url, book_json, created_date, published_date, page_count)
            VALUES (@isbn, @title, @author, @image_url, @book_json, @created_date, @published_date, @page_count)
            RETURNING id;";
        var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@isbn", book.isbn);
        cmd.Parameters.AddWithValue("@title", book.title);
        cmd.Parameters.AddWithValue("@author", book.author);
        cmd.Parameters.AddWithValue("@image_url", book.image_url);
        cmd.Parameters.AddWithValue("@book_json", NpgsqlDbType.Jsonb, book.book_json);
        cmd.Parameters.AddWithValue("@created_date", NpgsqlDbType.Timestamp, DateTime.Now);
        cmd.Parameters.AddWithValue("@published_date", NpgsqlDbType.Date, book.published_date);
        cmd.Parameters.AddWithValue("@page_count", NpgsqlDbType.Integer, book.page_count);
        var id = await cmd.ExecuteScalarAsync();
        return (int)id;
    }
    public async Task<Book?> GetBookByIsbn(string isbn) {
        await using var conn = await GetNewOpenConnection();
        var sql = @"
            SELECT * FROM public.books
            WHERE isbn = @isbn
        ";
        var book = await conn.QueryFirstOrDefaultAsync<Book>(sql, new{isbn});
        return book;
    }
    
    public async Task<List<Book>> GetBooksWithoutTheJson() {
        await using var conn = await GetNewOpenConnection();
        var sql = @"
            SELECT id, isbn, title, author, image_url, created_date, published_date, page_count FROM public.books
        ";
        var books = await conn.QueryAsync<Book>(sql);
        return books.ToList();
    }
}