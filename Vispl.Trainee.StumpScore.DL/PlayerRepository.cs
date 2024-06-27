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
                    // required
                    command.Parameters.AddWithValue("@JerseyNumber", player.JerseyNumber);
                    // required
                    command.Parameters.AddWithValue("@Name", player.Name);
                    // required
                    command.Parameters.AddWithValue("@DateOfBirth", player.DateOfBirth);
                    // required
                    command.Parameters.AddWithValue("@Age", player.Age);
                    // can be null
                    if (player.BirthPlace == null)
                    {
                        command.Parameters.AddWithValue("@BirthPlace", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BirthPlace", player.BirthPlace);
                    }
                    command.Parameters.AddWithValue("@CountryId", player.CountryId);
                    command.Parameters.AddWithValue("@PlayerTypeId", player.PlayerTypeId);
                    command.Parameters.AddWithValue("@IsCaptain", player.IsCaptain);

                    // nullable
                    if (player.MatchesPlayed == null)
                    {
                        command.Parameters.AddWithValue("@MatchesPlayed", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@MatchesPlayed", player.MatchesPlayed);
                    }
                    if (player.RunsScored == null)
                    {
                        command.Parameters.AddWithValue("@RunsScored", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@RunsScored", player.RunsScored);
                    }
                    if (player.WicketTaken == null)
                    {
                        command.Parameters.AddWithValue("@WicketTaken", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@WicketTaken", player.WicketTaken);
                    }
                    if (player.BattingAverage == null)
                    {
                        command.Parameters.AddWithValue("@BattingAverage", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BattingAverage", player.BattingAverage);
                    }
                    if (player.BowlingAverage == null)
                    {
                        command.Parameters.AddWithValue("@BowlingAverage", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@BowlingAverage", player.BowlingAverage);
                    }
                    if (player.Centuries == null)
                    {
                        command.Parameters.AddWithValue("@Centuries", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Centuries", player.Centuries);
                    }

                    if (player.HalfCenturies == null)
                    {
                        command.Parameters.AddWithValue("@HalfCenturies", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@HalfCenturies", player.HalfCenturies);
                    }

                    if (player.DebutDate == null)
                    {
                        command.Parameters.AddWithValue("@DebutDate", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DebutDate", player.DebutDate);
                    }

                    if (player.ICCRanking == null)
                    {
                        command.Parameters.AddWithValue("@ICCRanking", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ICCRanking", player.ICCRanking);
                    }



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
                                CountryName = row["CountryName"].ToString(),
                                CountryFlag = row["CountryFlag"].ToString()
                            },
                            PlayerType = new PlayerType
                            {
                                Id = Convert.ToInt32(row["PlayerTypeId"]),
                                TypeName = row["TypeName"].ToString()
                            },
                            IsCaptain = Convert.ToBoolean(row["IsCaptain"]),
                            Picture = row["Picture"].ToString()
                        };
                        // ------- handling for the null values

                        if (row["MatchesPlayed"] == DBNull.Value)
                        {
                            player.MatchesPlayed = null;
                        }
                        else
                        {
                            player.MatchesPlayed = Convert.ToInt32(row["MatchesPlayed"]);
                        }
                        if (row["RunsScored"] == DBNull.Value)
                        {
                            player.RunsScored = null;
                        }
                        else
                        {
                            player.RunsScored = Convert.ToInt32(row["RunsScored"]);
                        }
                        if (row["WicketTaken"] == DBNull.Value)
                        {
                            player.WicketTaken = null;
                        }
                        else
                        {
                            player.WicketTaken = Convert.ToInt32(row["WicketTaken"]);
                        }
                        if (row["BattingAverage"] == DBNull.Value)
                        {
                            player.BattingAverage = null;
                        }
                        else
                        {
                            player.BattingAverage = Convert.ToDouble(row["BattingAverage"]);
                        }
                        if (row["BowlingAverage"] == DBNull.Value)
                        {
                            player.BowlingAverage = null;
                        }
                        else
                        {
                            player.BowlingAverage = Convert.ToDouble(row["BowlingAverage"]);
                        }
                        if (row["Centuries"] == DBNull.Value)
                        {
                            player.Centuries = null;
                        }
                        else
                        {
                            player.Centuries = Convert.ToInt32(row["Centuries"]);
                        }
                        if (row["HalfCenturies"] == DBNull.Value)
                        {
                            player.HalfCenturies = null;
                        }
                        else
                        {
                            player.HalfCenturies = Convert.ToInt32(row["HalfCenturies"]);
                        }
                        if (row["DebutDate"] == DBNull.Value)
                        {
                            player.DebutDate = null;
                        }
                        else
                        {
                            player.DebutDate = Convert.ToDateTime(row["DebutDate"]);
                        }
                        if (row["ICCRanking"] == DBNull.Value)
                        {
                            player.ICCRanking = null;
                        }
                        else
                        {
                            player.ICCRanking = Convert.ToInt32(row["ICCRanking"]);
                        }

                        players.Add(player);
                    }
                }
            }
            return players;
        }
    }
}
