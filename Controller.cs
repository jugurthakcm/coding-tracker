using System.Diagnostics;
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

                // Table displaying all records
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

                // Declaration of the total and average coding session per period
                TimeSpan? totalTimeSpentCoding = TimeSpan.Zero;

                // Calculate the total time spent in coding
                foreach (var session in sessions)
                {
                    if (TimeSpan.TryParse(session.Duration, out TimeSpan parsedDuration))
                        totalTimeSpentCoding += parsedDuration;
                }

                TimeSpan? averageTimeSpentCoding = totalTimeSpentCoding / sessions.Count();

                // Display total and average times
                Console.WriteLine("");
                AnsiConsole.MarkupLine($"[bold]Total time spent coding:[/] {totalTimeSpentCoding}");
                AnsiConsole.MarkupLine(
                    $"[bold]Average time spent coding:[/] {averageTimeSpentCoding}"
                );

                connection.Close();
            }
        }

        // Insert a tracking session
        public static async Task Insert()
        {
            Console.Clear();

            CodingSession codingSession = await Utils.GetDateTimeFromUser();

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
            string? idToDelete = Console.ReadLine();

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
        public static async Task Update()
        {
            Console.Clear();

            GetAllRecords();

            Console.Write("\n\nInsert the ID of the coding session that you want to update: ");
            string? idToUpdate = Console.ReadLine();

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

                CodingSession codingSession = await Utils.GetDateTimeFromUser();

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

        // Track coding session using a stopwatch
        public static void StopWatch()
        {
            Stopwatch stopwatch = new Stopwatch();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine(
                    "Press 'S' to Start/Stop, 'R' to Reset, 'E' to Exit and Save Record."
                );
                AnsiConsole.MarkupLine(
                    $"Elapsed Time: [green]{stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}[/]"
                );

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        // Start and stop the stopwatch
                        case ConsoleKey.S:
                            if (stopwatch.IsRunning)
                            {
                                stopwatch.Stop();
                                Console.WriteLine("Stopwatch stopped.");
                            }
                            else
                            {
                                stopwatch.Start();
                                Console.WriteLine("Stopwatch started.");
                            }
                            break;

                        // Reset the stopwatch
                        case ConsoleKey.R:
                            stopwatch.Reset();
                            Console.WriteLine("Stopwatch reset.");
                            break;

                        // Exit the stopwatch and save record
                        case ConsoleKey.E:
                            running = false;

                            // Get today's date in format of MM/dd/yyyy
                            string todayDate = DateTime.Now.ToString("MM/dd/yyyy");

                            // Format the elapsed time (hh:mm:ss) to add it to the DB
                            TimeSpan elapsed = stopwatch.Elapsed;
                            string formattedElapsed = string.Format(
                                "{0:D2}:{1:D2}:{2:D2}",
                                elapsed.Hours,
                                elapsed.Minutes,
                                elapsed.Seconds
                            );

                            // Save to DB
                            using (
                                var connection = new SqliteConnection(Variables.defaultConnection)
                            )
                            {
                                string sql =
                                    @$"INSERT INTO coding_tracker(StartTime, EndTime, Duration) 
                                    VALUES('{todayDate}','{todayDate}','{formattedElapsed}')";

                                connection.Execute(sql);

                                connection.Close();
                            }

                            Console.WriteLine("Exiting and saving record...");
                            break;
                    }
                }

                // Delay to reduce CPU usage
                Thread.Sleep(100);
            }
        }
    }
}
