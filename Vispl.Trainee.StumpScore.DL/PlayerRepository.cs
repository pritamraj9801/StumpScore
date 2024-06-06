using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.VO.Enums;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.DL
{
    public class PlayerRepository
    {
        private readonly string _connectionString;
        public PlayerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool Add(Player player)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Player (JerseyNumber, Name, DateOfBirth, Age, BirthPlace, CountryId, PlayerTypeId, 
                             IsCaptain, MatchesPlayed, RunsScored, WicketTaken, BattingAverage, BowlingAverage, 
                             Centuries, HalfCenturies, DebutDate, ICCRanking, Picture)
                             VALUES (@JerseyNumber, @Name, @DateOfBirth, @Age, @BirthPlace, @CountryId, @PlayerTypeId, 
                             @IsCaptain, @MatchesPlayed, @RunsScored, @WicketTaken, @BattingAverage, @BowlingAverage, 
                             @Centuries, @HalfCenturies, @DebutDate, @ICCRanking, @Picture)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JerseyNumber", player.JerseyNumber);
                    command.Parameters.AddWithValue("@Name", player.Name);
                    command.Parameters.AddWithValue("@DateOfBirth", player.DateOfBirth);
                    command.Parameters.AddWithValue("@Age", player.Age);
                    command.Parameters.AddWithValue("@BirthPlace", player.BirthPlace);
                    command.Parameters.AddWithValue("@CountryId", player.CountryId);
                    command.Parameters.AddWithValue("@PlayerTypeId", player.PlayerTypeId);
                    command.Parameters.AddWithValue("@IsCaptain", player.IsCaptain);
                    command.Parameters.AddWithValue("@MatchesPlayed", player.MatchesPlayed);
                    command.Parameters.AddWithValue("@RunsScored", player.RunsScored);
                    command.Parameters.AddWithValue("@WicketTaken", player.WicketTaken);
                    command.Parameters.AddWithValue("@BattingAverage", player.BattingAverage);
                    command.Parameters.AddWithValue("@BowlingAverage", player.BowlingAverage);
                    command.Parameters.AddWithValue("@Centuries", player.Centuries);
                    command.Parameters.AddWithValue("@HalfCenturies", player.HalfCenturies);
                    command.Parameters.AddWithValue("@DebutDate", player.DebutDate);
                    command.Parameters.AddWithValue("@ICCRanking", player.ICCRanking);
                    command.Parameters.AddWithValue("@Picture", player.Picture);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        return result > 0;  // successfully if result > 0
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }

        // --------------- Data reader
        public List<Player> GetAll()
        {
            List<Player> players = new List<Player>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = @"select * from Player 
left join Country on Player.CountryId=Country.Id
left join PlayerType on Player.PlayerTypeId=PlayerType.Id;";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset);

                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        Player player = new Player
                        {
                            Id = Convert.ToInt32(row["ID"]),
                            JerseyNumber = Convert.ToInt32(row["JerseyNumber"]),
                            Name = row["Name"].ToString(),
                            DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                            Age = Convert.ToInt32(row["Age"]),
                            BirthPlace = row["BirthPlace"].ToString(),
                            Nationality = new Country
                            {
                                Id = Convert.ToInt32(row["CountryId"]),
                                CountryName = row["CountryName"].ToString()
                            },
                            PlayerType = new PlayerType
                            {
                                Id = Convert.ToInt32(row["PlayerTypeId"]),
                                TypeName = row["TypeName"].ToString()
                            },
                            IsCaptain = Convert.ToBoolean(row["IsCaptain"]),
                            MatchesPlayed = Convert.ToInt32(row["MatchesPlayed"]),
                            RunsScored = Convert.ToInt32(row["RunsScored"]),
                            WicketTaken = Convert.ToInt32(row["WicketTaken"]),
                            BattingAverage = Convert.ToDouble(row["BattingAverage"]),
                            BowlingAverage = Convert.ToDouble(row["BowlingAverage"]),
                            Centuries = Convert.ToInt32(row["Centuries"]),
                            HalfCenturies = Convert.ToInt32(row["HalfCenturies"]),
                            DebutDate = Convert.ToDateTime(row["DebutDate"]),
                            ICCRanking = Convert.ToInt32(row["ICCRanking"]),
                            Picture = row["Picture"].ToString()
                        };

                        players.Add(player);
                    }
                }
            }
            return players;
        }
    }
}
