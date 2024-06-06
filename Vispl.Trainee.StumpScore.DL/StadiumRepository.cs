using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.DL
{
    public class StadiumRepository
    {
        private readonly string _connectionString;
        public StadiumRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Stadium> GetAll()
        {
            List<Stadium> stadiums = new List<Stadium>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = "select * from stadium";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        Stadium stadium = new Stadium
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            StadiumName = row["StadiumName"].ToString()
                        };
                        stadiums.Add(stadium);
                    }
                }              
            }
            return stadiums;
        }
        public Stadium Get(int stadiumId)
        {
            Stadium stadium = new Stadium();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = $"select * from Stadium where Id = {stadiumId};";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        stadium = new Stadium
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            StadiumName = row["StadiumName"].ToString()
                        };
                    }
                }
            }
            return stadium;
        }
    }
}
