using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;

namespace ELearning.Infastructure.Data;
public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public DbConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
