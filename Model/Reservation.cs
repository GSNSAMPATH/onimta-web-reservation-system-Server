namespace OIT_Reservation.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ReservedBy { get; set; }
        public DateTime Date { get; set; }
        public string Purpose { get; set; }
    }
}
