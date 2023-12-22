using System.ComponentModel.DataAnnotations;

namespace ChinaCatSunflower.Models.Book;

public class AddBook
{
    [Required]
    [MinLength(7, ErrorMessage = "ISBN must be at least 7 characters long")]
    public string ISBN { get; set; }
}