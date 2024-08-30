using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker
{
    public static class Utils
    {

        public static CodingSession GetDateTimeFromUser()
        {

            // INSERT START TIME
            Console.WriteLine("\nInsert the time when you started coding MM/dd/yyyy hh:mm (AM/PM): ");

            string startDateTimeString = Console.ReadLine();

            DateTime startDateTime = Validation.StringToDateTime(startDateTimeString);

            // INSERT END TIME
            Console.WriteLine("\nInsert the time when you finished coding MM/dd/yyyy hh:mm (AM/PM): ");

            string endDateTimeString = Console.ReadLine();

            DateTime endDateTime = Validation.StringToDateTime(endDateTimeString);


            return new CodingSession(startDateTime, endDateTime);


        }
    }
}
