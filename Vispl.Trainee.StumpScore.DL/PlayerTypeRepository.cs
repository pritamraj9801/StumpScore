using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.VO.Enums;

namespace Vispl.Trainee.StumpScore.DL
{
    public class PlayerTypeRepository
    {
        private readonly string _connectionString;
        public PlayerTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<PlayerType> GetAll()
        {
            List<PlayerType> playerTypes = new List<PlayerType>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = "select * from PlayerType";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        PlayerType playerType = new PlayerType
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            TypeName = row["TypeName"].ToString()
                        };
                        playerTypes.Add(playerType);
                    }
                }
            }
            return playerTypes;
        }
    }
}
