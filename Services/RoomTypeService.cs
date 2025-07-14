using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OIT_Reservation.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace OIT_Reservation.Services
{
    public class RoomTypeService
    {
        private readonly string _conn;

        public RoomTypeService(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<RoomType> GetAll()
        {
            var list = new List<RoomType>();

            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("sp_GetAllRoomTypes", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new RoomType
                {
                    RoomTypeID = (int)reader["RoomTypeID"],
                    RoomTypeCode = reader["RoomTypeCode"].ToString(),
                    Description = reader["Description"].ToString(),
                    Remarks = reader["Remarks"].ToString(),
                    IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : true
                });
            }

            return list;
        }

        public bool Create(RoomType roomType)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_reservation_roomtype_save", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input params
                cmd.Parameters.AddWithValue("@RoomTypeCode", roomType.RoomTypeCode ?? string.Empty);
                cmd.Parameters.AddWithValue("@RoomTypeName", roomType.Description ?? string.Empty);
                cmd.Parameters.AddWithValue("@Remark", roomType.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", roomType.IsActive);
                cmd.Parameters.AddWithValue("@IsNew", true); // true for create

                // Output param
                var outputParam = new SqlParameter("@RoomTypeCodeRet", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                string returnedCode = outputParam.Value?.ToString() ?? string.Empty;
                return !string.IsNullOrEmpty(returnedCode);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000 && ex.Message.Contains("Room Type Code already exists"))
                    throw new ApplicationException("Room Type Code already exists.");

                throw new ApplicationException("Database error: " + ex.Message);
            }
        }

        public bool Update(RoomType roomType)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_reservation_roomtype_save", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input params
                cmd.Parameters.AddWithValue("@RoomTypeCode", roomType.RoomTypeCode ?? string.Empty);
                cmd.Parameters.AddWithValue("@RoomTypeName", roomType.Description ?? string.Empty);
                cmd.Parameters.AddWithValue("@Remark", roomType.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", roomType.IsActive);
                cmd.Parameters.AddWithValue("@IsNew", false); // false for update

                // Output param
                var outputParam = new SqlParameter("@RoomTypeCodeRet", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                string returnedCode = outputParam.Value?.ToString() ?? string.Empty;
                return !string.IsNullOrEmpty(returnedCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating room type: " + ex.Message);
            }
        }
    }
}
