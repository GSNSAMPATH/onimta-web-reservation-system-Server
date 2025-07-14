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
                });
            }

            return list;
        }


        // public RoomType GetById(int id)
        // {
        //     using var conn = new SqlConnection(_conn);
        //     using var cmd = new SqlCommand("sp_GetRoomTypeById", conn)
        //     {
        //         CommandType = CommandType.StoredProcedure
        //     };
        //     cmd.Parameters.AddWithValue("@RoomTypeID", id);
        //     conn.Open();
        //     using var reader = cmd.ExecuteReader();
        //     if (reader.Read())
        //     {
        //         return new RoomType
        //         {
        //             RoomTypeID = (int)reader["RoomTypeID"],
        //             RoomTypeCode = reader["RoomTypeCode"].ToString(),
        //             RoomTypeName = reader["RoomTypeName"].ToString(),
        //             Remark = reader["Remark"].ToString(),
        //             CreateDate = reader["CreateDate"] != DBNull.Value ? (DateTime)reader["CreateDate"] : DateTime.MinValue,
        //             UpdateDate = reader["UpdateDate"] != DBNull.Value ? (DateTime?)reader["UpdateDate"] : null,
        //             CreatedBy = reader["CreatedBy"]?.ToString(),
        //             UpdatedBy = reader["UpdatedBy"]?.ToString()
        //         };
        //     }
        //     return null;
        // }

public bool Create(RoomType roomType)
{
    try
    {
        using var conn = new SqlConnection(_conn);
        using var cmd = new SqlCommand("sp_InsertRoomType", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@RoomTypeCode", roomType.RoomTypeCode);
        cmd.Parameters.AddWithValue("@Description", roomType.Description);
        cmd.Parameters.AddWithValue("@Remarks", roomType.Remarks ?? (object)DBNull.Value);

        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
    catch (SqlException ex)
    {
        // Optional: log error
        // If it's the duplicate error you expect, you can handle it
        if (ex.Number == 50000 && ex.Message.Contains("Room Type Code already exists"))
            throw new ApplicationException("Room Type Code already exists.");

        // Otherwise rethrow or handle
        throw new ApplicationException("Database error: " + ex.Message);
    }
}


        public bool Update(RoomType roomType)
        {
            Console.WriteLine(roomType.RoomTypeID);
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("sp_UpdateRoomType", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@RoomTypeID", roomType.RoomTypeID);
            cmd.Parameters.AddWithValue("@RoomTypeCode", roomType.RoomTypeCode);
            cmd.Parameters.AddWithValue("@Description", roomType.Description);
            cmd.Parameters.AddWithValue("@Remarks", roomType.Remarks ?? (object)DBNull.Value);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_DeleteRoomType", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Input parameter
                cmd.Parameters.AddWithValue("@RoomTypeID", id);

                // Output parameter
                var resultParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(resultParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                int resultCode = (int)resultParam.Value;

                if (resultCode == -1)
                    throw new ApplicationException("Room Type not found.");

                return true;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Database error: " + ex.Message);
            }
        }



    }
}

