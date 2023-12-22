using System.ComponentModel.DataAnnotations.Schema;

namespace ChinaCatSunflower.Data.Models;

[Table("fib_log")]
public class FibLog
{
    public int id { get; set; }
    public long fib { get; set; }
    public string? user_name { get; set; }
    public DateTime? created_date { get; set; }
}