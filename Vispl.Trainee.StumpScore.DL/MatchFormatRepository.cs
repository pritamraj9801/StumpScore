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
    public class MatchFormatRepository
    {
        private readonly string _connectionString;
        public MatchFormatRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<MatchFormat> GetAll()
        {
            List<MatchFormat> matchFormats = new List<MatchFormat>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = "select * from MatchFormat";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset);
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        MatchFormat matchFormat = new MatchFormat
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Name = row["Name"].ToString()
                        };
                        matchFormats.Add(matchFormat);
                    }
                }
            }
            return matchFormats;
        }
    }
}
