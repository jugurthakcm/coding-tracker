using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace coding_tracker
{
    public static class Utils
    {
        public static async Task<CodingSession> GetDateTimeFromUser()
        {
            // INSERT START TIME
            Console.WriteLine(
                "\nInsert the time when you started coding MM/dd/yyyy hh:mm (AM/PM): "
            );

         
            string recognizedText = await Speech.HandleSpeech();
            string startDateTimeString = recognizedText.Replace(",", "").Replace(".", "");

            DateTime startDateTime = await Validation.StringToDateTime(startDateTimeString);

            // INSERT END TIME
            Console.WriteLine(
                "\nInsert the time when you finished coding MM/dd/yyyy hh:mm (AM/PM): "
            );

             recognizedText = await Speech.HandleSpeech();
            string endDateTimeString = recognizedText.Replace(",", "").Replace(".", "");

            DateTime endDateTime = await Validation.StringToDateTime(endDateTimeString);

            return new CodingSession(startDateTime, endDateTime);
        }
    }
}
