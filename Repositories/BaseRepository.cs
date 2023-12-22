using System.Data;
using ChinaCatSunflower.AppHelpers;
using Dapper.Contrib.Extensions;
using Npgsql;

namespace ChinaCatSunflower.Repositories;

public class BasePgRepository
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly string _app_name;
    private readonly string? _connection_string;

    public BasePgRepository(IConfiguration configuration, ILogger logger) {
        _configuration = configuration;
        _logger = logger;
        _app_name = ApplicationSettings.APPLICATION_NAME;
        _connection_string = configuration.GetConnectionString(ApplicationSettings.SQL_CONNECTION_NAME);
    }

    public async Task<int> Insert<T>(T? data) where T: class
    {
        if (data == null) {
            return -1;
        }
        await using var conn = await GetNewOpenConnection();
        //
        // if (data is IDataModel data_as_datamodel) {
        //     data_as_datamodel.updated_by = _username_retriever.Username;
        //     data_as_datamodel.updated_on = DateTime.UtcNow;
        //     data_as_datamodel.created_by = _username_retriever.Username;
        //     data_as_datamodel.created_on = DateTime.UtcNow;
        // }
        //     
        var rv = await conn.InsertAsync(data);
        _logger.LogInformation($"Inserted FibLog with success flag of {rv}");
        return rv;
    }
    public async Task<bool> Update<T>(T? data) where T: class
    {
        if (data == null) {
            return false;
        }
        await using var conn = await GetNewOpenConnection();
            
        // if (data is IDataModel data_as_datamodel) {
        //     data_as_datamodel.updated_by = _username_retriever.Username;
        //     data_as_datamodel.updated_on = DateTime.UtcNow;
        // }
        var rv = await conn.UpdateAsync(data);
        return rv;
    }

    protected async Task<NpgsqlConnection> GetNewOpenConnection() {
        var sql_connection = new NpgsqlConnection(_connection_string);
        if (sql_connection.State != ConnectionState.Open)
        {
            await sql_connection.OpenAsync();
        }
        return sql_connection;
    }
    
}