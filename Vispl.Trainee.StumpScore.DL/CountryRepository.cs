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
    public class CountryRepository
    {
        private readonly string _connectionString;
        public CountryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Country> GetAll()
        {
            List<Country> countries = new List<Country>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = "select * from Country";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                  
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset);
                    foreach(DataRow row in dataset.Tables[0].Rows)
                    {
                        Country country = new Country
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            CountryName = row["CountryName"].ToString(),
                            CountryFlag = row["CountryFlag"].ToString()
                        };
                        countries.Add(country);
                    }
                }
            }
            return countries;
        }
    }
}
