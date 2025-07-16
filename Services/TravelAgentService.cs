using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OIT_Reservation.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace OIT_Reservation.Services
{
    public class TravelAgentService
    {
        private readonly string _conn;

        public TravelAgentService(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public bool Create(TravelAgent travelAgent)
        {
            try
            {
                using var conn = new SqlConnection(_conn);
                using var cmd = new SqlCommand("sp_InsertTravelAgent", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Output param for generated code
                var travelAgentCodeParam = new SqlParameter("@TravelAgentCode", SqlDbType.NVarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(travelAgentCodeParam);
                cmd.Parameters.AddWithValue("@Description", travelAgent.Description);

                conn.Open();
                cmd.ExecuteNonQuery();

                // Set the generated RoomTypeCode to return or log
                travelAgent.TravelAgentCode = travelAgentCodeParam.Value.ToString();

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000 && ex.Message.Contains("Travle Agent Code already exists"))
                    throw new ApplicationException("Travle Agent Code already exists.");

                throw new ApplicationException("Database error: " + ex.Message);
            }
        }
    }
}