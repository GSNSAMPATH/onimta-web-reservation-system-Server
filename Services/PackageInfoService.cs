using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OIT_Reservation.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace OIT_Reservation.Services
{
    public class PackageInfoService
    {
        private readonly string _conn;

        public PackageInfoService(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public List<PackageInfo> GetAll()
        {
            var list = new List<PackageInfo>();

            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("sp_reservation_package_getall", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new PackageInfo
                {
                    PackageID = Convert.ToInt32(reader["PackageID"]),
                    PackageCode = reader["PackageCode"].ToString(),
                    PackageName = reader["PackageName"].ToString(),
                    PackageDuration = Convert.ToDecimal(reader["PackageDuration"]),
                    Remarks = reader["Remarks"] != DBNull.Value ? reader["Remarks"].ToString() : null,

                    RoomPrice = Convert.ToDecimal(reader["RoomPrice"]),
                    RoomCost = Convert.ToDecimal(reader["RoomCost"]),
                    RoomAmount = Convert.ToDecimal(reader["RoomAmount"]),
                    FoodAmount = Convert.ToDecimal(reader["FoodAmount"]),
                    BeverageAmount = Convert.ToDecimal(reader["BeverageAmount"]),

                    IsRoom = Convert.ToBoolean(reader["IsRoom"]),
                    IsBanquet = Convert.ToBoolean(reader["IsBanquet"])
                });
            }
            return list;
        }

        public bool Create(PackageInfo packageInfo)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_reservation_package_save", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // PackageCode is input/output
                var packageCodeParam = new SqlParameter("@PackageCode", SqlDbType.VarChar, 300)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = string.IsNullOrEmpty(packageInfo.PackageCode) ? (object)DBNull.Value : packageInfo.PackageCode
                };
                cmd.Parameters.Add(packageCodeParam);

                // Output parameter for return value
                var packageCodeRetParam = new SqlParameter("@PackageCodeRet", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(packageCodeRetParam);

                cmd.Parameters.AddWithValue("@PackageName", packageInfo.PackageName);
                cmd.Parameters.AddWithValue("@PackageDuration", packageInfo.PackageDuration);
                cmd.Parameters.AddWithValue("@Remark", string.IsNullOrEmpty(packageInfo.Remarks) ? (object)DBNull.Value : packageInfo.Remarks);

                cmd.Parameters.AddWithValue("@RoomPrice", packageInfo.RoomPrice);
                cmd.Parameters.AddWithValue("@RoomCost", packageInfo.RoomCost);
                cmd.Parameters.AddWithValue("@RoomAmount", packageInfo.RoomAmount);
                cmd.Parameters.AddWithValue("@FoodAmount", packageInfo.FoodAmount);
                cmd.Parameters.AddWithValue("@BeverageAmount", packageInfo.BeverageAmount);

                cmd.Parameters.AddWithValue("@IsRoom", packageInfo.IsRoom);
                cmd.Parameters.AddWithValue("@IsBanquet", packageInfo.IsBanquet);

                conn.Open();
                cmd.ExecuteNonQuery();

                // Capture the returned/generated code (either works)
                packageInfo.PackageCode = packageCodeRetParam.Value?.ToString() ?? packageCodeParam.Value?.ToString();

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000 && ex.Message.Contains("Package Information Code already exists"))
                    throw new ApplicationException("Package Information Code already exists.");

                throw new ApplicationException("Database error: " + ex.Message);
            }
        }

        public bool Update(PackageInfo packageInfo)
{
    using var conn = new SqlConnection(_conn);
    using var cmd = new SqlCommand("sp_reservation_package_save", conn)
    {
        CommandType = CommandType.StoredProcedure
    };

    // Send PackageID (0 for insert, >0 for update)
    cmd.Parameters.AddWithValue("@PackageID", packageInfo.PackageID);

    // Output/input PackageCode
    var packageCodeParam = new SqlParameter("@PackageCode", SqlDbType.VarChar, 300)
    {
        Direction = ParameterDirection.InputOutput,
        Value = (object?)packageInfo.PackageCode ?? string.Empty
    };
    cmd.Parameters.Add(packageCodeParam);

    // Output returned code
    var packageCodeRetParam = new SqlParameter("@PackageCodeRet", SqlDbType.VarChar, 20)
    {
        Direction = ParameterDirection.Output
    };
    cmd.Parameters.Add(packageCodeRetParam);

    // Other parameters
    cmd.Parameters.AddWithValue("@PackageName", packageInfo.PackageName);
    cmd.Parameters.AddWithValue("@PackageDuration", packageInfo.PackageDuration);
    cmd.Parameters.AddWithValue("@Remark", string.IsNullOrEmpty(packageInfo.Remarks) ? DBNull.Value : packageInfo.Remarks);
    cmd.Parameters.AddWithValue("@RoomPrice", packageInfo.RoomPrice);
    cmd.Parameters.AddWithValue("@RoomCost", packageInfo.RoomCost);
    cmd.Parameters.AddWithValue("@RoomAmount", packageInfo.RoomAmount);
    cmd.Parameters.AddWithValue("@FoodAmount", packageInfo.FoodAmount);
    cmd.Parameters.AddWithValue("@BeverageAmount", packageInfo.BeverageAmount);
    cmd.Parameters.AddWithValue("@IsRoom", packageInfo.IsRoom);
    cmd.Parameters.AddWithValue("@IsBanquet", packageInfo.IsBanquet);

    conn.Open();

    int rowsAffected = 0;

    using (var reader = cmd.ExecuteReader())
    {
        if (reader.Read() && reader["RowsAffected"] != DBNull.Value)
            rowsAffected = Convert.ToInt32(reader["RowsAffected"]);
    }

    // Update the object with generated code (insert or update)
    packageInfo.PackageCode = packageCodeRetParam.Value?.ToString();

    return rowsAffected > 0;
}


    }
}
