namespace ChinaCatSunflower.Data.Models;

public class FibCountRecord
{
    public long fib { get; set; }
    public string Fibs => fib.ToString("#,##0");
    public int times { get; set; }
}