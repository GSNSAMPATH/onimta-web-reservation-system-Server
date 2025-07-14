// File: Services/IReservationRoomService.cs
using OIT_Reservation.Models;

namespace OIT_Reservation.Services
{
    public interface IReservationRoomService
    {
        string SaveRoom(ReservationRoom room);
    }
}
