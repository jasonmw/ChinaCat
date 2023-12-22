using ChinaCatSunflower.Data.Models;
using Dapper;

namespace ChinaCatSunflower.Repositories;

public class FibLogRepository : BasePgRepository
{
    private readonly ILogger<FibLogRepository> _logger;

    public FibLogRepository(IConfiguration configuration, ILogger<FibLogRepository> logger) : base(configuration, logger) {
        _logger = logger;
    }

    public async Task Add(long fib, string? username) {
        var fib_log = new FibLog() {
            fib = fib,
            user_name = username,
            created_date = DateTime.UtcNow
        }; 
        await Insert(fib_log);
        _logger.LogInformation($"Fib inserted was {fib:#,##0}");
    }

    public async Task<List<FibCountRecord>> GetFibLogCounts() {
        await using var conn = await GetNewOpenConnection();
        var sql = @"
SELECT fib, COUNT(id) as times FROM fib_log GROUP BY fib HAVING COUNT(id) > 1  ORDER BY COUNT(id) DESC, fib DESC;
        ";
        var rv = await conn.QueryAsync<FibCountRecord>(sql);
        return rv.ToList();
    }
}