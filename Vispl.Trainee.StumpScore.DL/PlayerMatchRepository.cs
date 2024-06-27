using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.DL
{
    public class PlayerMatchRepository
    {
        private readonly string _connectionString;
        public PlayerMatchRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool Add(int[] players, int tournamentId, int teamId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO PlayerMatch (PlayerId, TeamId, TournamentId)
                        VALUES (@PlayerId, @TeamId, @TournamentId)";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@PlayerId", SqlDbType.Int);
                    sqlCommand.Parameters.Add("@TeamId", SqlDbType.Int);
                    sqlCommand.Parameters.Add("@TournamentId", SqlDbType.Int);

                    try
                    {
                        sqlConnection.Open();

                        foreach (int playerId in players)
                        {
                            sqlCommand.Parameters["@PlayerId"].Value = playerId;
                            sqlCommand.Parameters["@TeamId"].Value = teamId;
                            sqlCommand.Parameters["@TournamentId"].Value = tournamentId;

                            int rowsAffected = sqlCommand.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                // If no rows were affected, insertion failed for this player
                                return false;
                            }
                        }

                        // If loop completes without returning, all players were successfully inserted
                        return true;
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
        public List<Player> GetPlayersForMatch(int teamId, int tournamentId)
        {
            List<Player> players = new List<Player>();
            using(SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = $"select\nPlayer.Id as 'PlayerId',\nPlayer.JerseyNumber,\nPlayer.Name,\nPlayer.DateOfBirth,\nPlayer.Age,\nPlayer.BirthPlace,\nPlayer.CountryId,\nPlayer.PlayerTypeId,\nPlayer.IsCaptain,\nPlayer.MatchesPlayed,\nPlayer.RunsScored,\nPlayer.WicketTaken,\nPlayer.BattingAverage,\nPlayer.BowlingAverage,\nPlayer.Centuries,\nPlayer.HalfCenturies,\nPlayer.DebutDate,\nPlayer.ICCRanking,\nPlayer.Picture\nfrom Playermatch\nleft join Player on Playermatch.PlayerId = Player.id\nwhere teamId = {teamId} and TournamentId={tournamentId};";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        Player player = new Player
                        {
                            Id = Convert.ToInt32(row["PlayerId"]),
                            JerseyNumber = Convert.ToInt32(row["JerseyNumber"]),
                            Name = row["Name"].ToString(),
                            PlayerTypeId = Convert.ToInt32(row["PlayerTypeId"])
                        };
                        players.Add(player);
                    }
                }
            }
            return players;
        }
    }
}
