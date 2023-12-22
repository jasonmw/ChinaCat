namespace ChinaCatSunflower.Data.Models;

public class Book
{
    public int id { get; private set; } // Assuming 'id' is auto-generated and not set manually
    public string isbn { get; set; }
    public string title { get; set; }
    public string author { get; set; }
    public string image_url { get; set; } // Matching the exact column name 'image_url'
    public string book_json { get; set; } // As 'book_json' in the table
    public DateTime created_date { get; set; } // As 'created_date' in the table
    public DateTime published_date { get; set; } // As 'published_date' in the table
    public int page_count { get; set; } // As 'page_count' in the table

    // Constructor and any necessary methods can go here
}