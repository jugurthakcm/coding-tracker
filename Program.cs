using System.Configuration;
using System.Collections.Specialized;
using Dapper;
using Microsoft.Data.Sqlite;
using coding_tracker;




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

Menu.ShowMenu();
