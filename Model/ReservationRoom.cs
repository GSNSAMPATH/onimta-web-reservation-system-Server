namespace OIT_Reservation.Models
{
    public class ReservationRoom
    {
        public string RoomTypeCode { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public int RoomSize { get; set; }
        public string RoomStatus { get; set; }
        public string Remark { get; set; }
        public bool IsRoom { get; set; }
        public bool IsBanquet { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
    }
}
