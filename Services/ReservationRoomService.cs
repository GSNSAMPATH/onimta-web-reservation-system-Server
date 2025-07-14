using OIT_Reservation.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace OIT_Reservation.Services
{
    public class ReservationRoomService : IReservationRoomService
    {
        private readonly string _conn;

        public ReservationRoomService(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public string SaveRoom(ReservationRoom room)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("sp_reservation_room_save", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@RoomTypeCode", room.RoomTypeCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RoomCode", room.RoomCode ?? "");
            cmd.Parameters.AddWithValue("@RoomName", room.RoomName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@RoomSize", room.RoomSize);
            cmd.Parameters.AddWithValue("@RoomStatus", room.RoomStatus ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Remark", room.Remark ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IsRoom", room.IsRoom);
            cmd.Parameters.AddWithValue("@IsBanquet", room.IsBanquet);
            cmd.Parameters.AddWithValue("@IsActive", room.IsActive);
            cmd.Parameters.AddWithValue("@IsNew", room.IsNew);

            var output = new SqlParameter("@RoomCodeRet", SqlDbType.VarChar, 20)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            conn.Open();
            cmd.ExecuteNonQuery();

            return output.Value?.ToString();
        }
    }
}
