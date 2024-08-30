using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace coding_tracker
{
    public class Controller
    {
        // View All tracking records
        public static void GetAllRecords()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(Variables.defaultConnection))
            {
                string sql = "SELECT * from coding_tracker";

                var sessions = connection.Query(sql);

                if (!sessions.Any())
                {
                    Console.WriteLine("No coding sessions found!");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }

                var table = new Table();

                table.AddColumn("[yellow]ID[/]");
                table.AddColumn("[yellow]Start Time[/]");
                table.AddColumn("[yellow]End Time[/]");
                table.AddColumn("[yellow]Duration[/]");

                foreach (var session in sessions)
                {
                    table.AddRow(
                        $"{session.Id}",
                        $"{session.StartTime}",
                        $"{session.EndTime}",
                        $"{session.Duration}"
                    );
                }

                AnsiConsole.Write(table);

                connection.Close();
            }
        }

        // Insert a tracking session
        public static void Insert()
        {
            Console.Clear();

            CodingSession codingSession = Utils.GetDateTimeFromUser();

            // INSERT INTO DB
            using (var connection = new SqliteConnection(Variables.defaultConnection))
            {
                string sql =
                    @$"INSERT INTO coding_tracker(StartTime, EndTime, Duration) 
                    VALUES('{codingSession.StartDateTime}','{codingSession.EndDateTime}',
                    '{codingSession.Duration}')";

                connection.Execute(sql);

                connection.Close();
            }

            AnsiConsole.MarkupLine("\n\n[green]Your tracking time has been added successfully![/]");
            Console.WriteLine("\nPress any key to continue....");
            Console.ReadLine();

            Console.Clear();
        }

        // Delete a tracking session
        public static void Delete()
        {
            Console.Clear();

            GetAllRecords();

            Console.Write("\n\nInsert the ID of the coding session that you want to delete: ");
            string idToDelete = Console.ReadLine();

            using (var connection = new SqliteConnection(Variables.defaultConnection))
            {
                string sql = $"DELETE FROM coding_tracker WHERE Id={idToDelete}";

                connection.Execute(sql);

                connection.Close();
            }

            AnsiConsole.MarkupLine($"\n\n[green]Record with Id {idToDelete} was deleted.[/]");
            Console.WriteLine("\nPress any key to continue....");
            Console.ReadLine();

            Console.Clear();
        }

        // Update a tracking session
        public static void Update()
        {
            Console.Clear();

            GetAllRecords();

            Console.Write("\n\nInsert the ID of the coding session that you want to update: ");
            string idToUpdate = Console.ReadLine();

            using (var connection = new SqliteConnection(Variables.defaultConnection))
            {
                string sql = $"SELECT * FROM coding_tracker WHERE Id={idToUpdate}";


                var session = connection.QuerySingleOrDefault(sql);

                if (session == null)
                {
                    AnsiConsole.MarkupLine(
                        $"\n\n[red]The session with Id {idToUpdate} doesn't exist[/]"
                    );

                    Console.ReadLine();

                    Update();
                }



                CodingSession codingSession = Utils.GetDateTimeFromUser();

                sql =
                    @$"UPDATE coding_tracker SET StartTime ='{codingSession.StartDateTime}', 
                    EndTime='{codingSession.EndDateTime}', Duration='{codingSession.Duration}' 
                    WHERE Id={idToUpdate} ";

                connection.Execute(sql);

                connection.Close();
            }

            AnsiConsole.MarkupLine($"\n\n[green]Record with Id {idToUpdate} was updated.[/]");
            Console.WriteLine("\nPress any key to continue....");
            Console.ReadLine();

            Console.Clear();
        }
    }
}
