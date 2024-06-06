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
    public class TournamentRepository
    {
        private readonly string _connectionString;
        public TournamentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Tournament> GetAll()
        {
            List<Tournament> tournaments = new List<Tournament>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = "select * from Tournament;";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();    
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        Tournament tournament = new Tournament
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Name = row["Name"].ToString(),
                            ShortName = row["ShortName"].ToString(),
                            StartingDate = Convert.ToDateTime(row["StartingDate"]),
                            EndingDate = Convert.ToDateTime(row["EndingDate"]),
                            MatchFormatId = Convert.ToInt32(row["MatchFormatId"])
                        };
                        tournaments.Add(tournament);
                    }
                }
            }
            return tournaments;
        }
        public bool Add(Tournament tournament)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Tournament (Name, ShortName, StartingDate, EndingDate, MatchFormatId, TournamentIcon)
                  VALUES (@Name, @ShortName, @StartingDate, @EndingDate, @MatchFormatId, @TournamentIcon)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", tournament.Name);
                    command.Parameters.AddWithValue("@ShortName", tournament.ShortName);
                    command.Parameters.AddWithValue("@StartingDate", tournament.StartingDate);
                    command.Parameters.AddWithValue("@EndingDate", tournament.EndingDate);
                    command.Parameters.AddWithValue("@MatchFormatId", tournament.MatchFormatId);
                    if (tournament.TournamentIcon != null)
                    {
                        command.Parameters.AddWithValue("@TournamentIcon", tournament.TournamentIcon);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@TournamentIcon", DBNull.Value);
                    }
                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as needed
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
