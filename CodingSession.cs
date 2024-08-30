public class CodingSession
{
 
    public CodingSession(DateTime startDateTime, DateTime endDateTime)
    {
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Duration = endDateTime - startDateTime;
    }

    public int Id { get; set; }

    public  DateTime StartDateTime { get; set; }

    public  DateTime EndDateTime { get; set; }

    public  TimeSpan Duration { get; set; }
}
