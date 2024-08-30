﻿namespace coding_tracker
{
    public class Menu
    {
        public static void ShowMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("Type 5 to Use the Stopwatch.");
                Console.WriteLine("------------------------------------------\n");

                string? command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Controller.GetAllRecords();
                        break;
                    case "2":
                        Controller.Insert();
                        break;
                    case "3":
                        Controller.Delete();
                        break;
                    case "4":
                        Controller.Update();
                        break;

                    case "5":
                        Controller.StopWatch();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }
    }
}
