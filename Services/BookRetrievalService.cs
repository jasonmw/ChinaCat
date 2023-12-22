using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ChinaCatSunflower.AppHelpers;
using ChinaCatSunflower.Data.Models;

namespace ChinaCatSunflower.Services;

public class BookRetrievalService
{
    private readonly IHttpClientFactory _http_client_factory;

    private readonly ILogger<BookRetrievalService> _logger;
    // /api/books?bibkeys=ISBN:1401309615&jscmd=details&format=json
    
    // 
    public BookRetrievalService(IHttpClientFactory httpClientFactory, ILogger<BookRetrievalService> logger) {
        _http_client_factory = httpClientFactory;
        _logger = logger;
    }
    
    public async Task<Book?> GetBookByIsbn(string isbn) {
        var client = _http_client_factory.CreateClient(ApplicationSettings.HTTP_CLIENT_OPEN_LIBRARY);
        var response = await client.GetAsync($"/api/books?bibkeys=ISBN:{isbn}&jscmd=details&format=json");
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Retrieved book from OpenLibrary: {isbn}, content: {content}", isbn, content);
        var json_nodes = JsonNode.Parse(content)?.AsObject();
        if (json_nodes.Count < 1) {
            return null;
        }
        var book_json = GetBookJson(json_nodes);
        if (book_json is null) {
            return null;
        }
        BookRetrieval? book_retrieval = JsonSerializer.Deserialize<BookRetrieval>(book_json);
        if (book_retrieval is null) {
            return null;
        }

        if (book_retrieval.details.publish_date.Length < 5 && int.TryParse(book_retrieval.details.publish_date, out _)) {
            book_retrieval.details.publish_date = $"1/1/{book_retrieval.details.publish_date}";
        }
        var book = new Book {
            book_json = book_json,
            isbn = isbn,
            title = book_retrieval.details.title,
            author = book_retrieval.details.authors.First().name,
            image_url = book_retrieval.thumbnail_url?.Replace("-S.", "-L.")??"",
            page_count = book_retrieval.details.number_of_pages,
            published_date = DateTime.Parse(book_retrieval.details.publish_date)
        };
        return book;
    }

    private string? GetBookJson(JsonObject? node) {
        if (node is null) {
            return null;
        }
        return node.First().Value?.ToJsonString();
    }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Author
    {
        public string key { get; set; }
        public string name { get; set; }
    }

    public class Created
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class Details
    {
        public List<string> publishers { get; set; }
        public string subtitle { get; set; }
        public string weight { get; set; }
        public List<int> covers { get; set; }
        public string physical_format { get; set; }
        public string key { get; set; }
        public List<Author> authors { get; set; }
        public string ocaid { get; set; }
        public List<string> subjects { get; set; }
        public List<string> source_records { get; set; }
        public string title { get; set; }
        public int number_of_pages { get; set; }
        public List<string> isbn_13 { get; set; }
        public List<string> isbn_10 { get; set; }
        public string publish_date { get; set; }
        public List<string> oclc_numbers { get; set; }
        public List<Work> works { get; set; }
        public Type type { get; set; }
        public string physical_dimensions { get; set; }
        public List<string> local_id { get; set; }
        public int latest_revision { get; set; }
        public int revision { get; set; }
        public Created created { get; set; }
        public LastModified last_modified { get; set; }
    }

    public class LastModified
    {
        public string type { get; set; }
        public DateTime value { get; set; }
    }

    public class BookRetrieval
    {
        public string bib_key { get; set; }
        public string info_url { get; set; }
        public string preview { get; set; }
        public string? preview_url { get; set; }
        public string? thumbnail_url { get; set; }
        public Details details { get; set; }
    }

    public class Type
    {
        public string key { get; set; }
    }

    public class Work
    {
        public string key { get; set; }
    }

