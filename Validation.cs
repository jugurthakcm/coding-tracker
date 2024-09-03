using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace coding_tracker
{
    public static class Validation
    {
        public static async Task<DateTime> StringToDateTime(string dateTimeString)
        {
            string format = "MM/dd/yyyy hh:mm tt";

            DateTime dateTimeOutput;

            while (
                !DateTime.TryParseExact(
                    dateTimeString,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateTimeOutput
                )
            )
            {
                AnsiConsole.MarkupLine("[red]Time isn't in the correct format[/]");

                Console.WriteLine(
                    "\nInsert the time in the correct format MM/dd/yyyy hh:mm (AM/PM): "
                );

                string recognizedText = await Speech.HandleSpeech();
                dateTimeString = recognizedText.Replace(",", "").Replace(".", "");
            }

            return dateTimeOutput;
        }
    }
}
