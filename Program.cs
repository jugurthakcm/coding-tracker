using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using coding_tracker;
using Dapper;
using Microsoft.Data.Sqlite;

using (var connection = new SqliteConnection(Variables.defaultConnection))
{
    var sql =
        @"CREATE TABLE IF NOT EXISTS coding_tracker (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration INTEGER
                        )";

    connection.Execute(sql);
}

await Menu.ShowMenu();
