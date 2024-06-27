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
    public class BallStatsRepository
    {
        private readonly string _connectionString;
        public BallStatsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<BallStats> GetAll(int matchId, int tournamentId,int inning)
        {
            List<BallStats> ballStats = new List<BallStats>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = $"select * from BallStats where MatchId={matchId} and TournamentId ={tournamentId} and Inning={inning}";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset);
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        BallStats ballStat = new BallStats();
                        ballStat.Id = Convert.ToInt32(row["Id"]);
                        ballStat.MatchId = Convert.ToInt32(row["MatchId"]);
                        ballStat.TournamentId = Convert.ToInt32(row["TournamentId"]);
                        ballStat.Inning = Convert.ToInt32(row["Inning"]);
                        ballStat.OnStrike = Convert.ToInt32(row["OnStrikePlayerId"]);
                        ballStat.OnNonStrike = Convert.ToInt32(row["OnNonStrikePlayerId"]);
                        ballStat.Bowler = Convert.ToInt32(row["BowlerId"]);
                        if (row["Runs"] != DBNull.Value)
                        {
                            ballStat.Runs = Convert.ToInt32(row["Runs"]);
                        }
                        ballStat.Extras = row["Extras"].ToString();
                        ballStat.Wickets = row["Wicket"].ToString();
                        if (row["FielderContributionId"] != DBNull.Value)
                        {
                            ballStat.FielderContribution = Convert.ToInt32(row["FielderContributionId"]);
                        }
                        ballStat.Commentatory = row["Commentatory"].ToString();
                        ballStats.Add(ballStat);
                    }
                }
            }
            return ballStats;
        }
        public bool Add(BallStats ballStats)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO BallStats (MatchId, TournamentId, Inning, OnStrikePlayerId, OnNonStrikePlayerId, BowlerId, 
                                                Runs, Extras, Wicket, FielderContributionId, Commentatory)
                         VALUES (@MatchId, @TournamentId, @Inning, @OnStrikePlayerId, @OnNonStrikePlayerId, @BowlerId,
                                 @Runs, @Extras, @Wicket, @FielderContributionId, @Commentatory)";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@MatchId", ballStats.MatchId == 0 ? DBNull.Value : (object)ballStats.MatchId);
                    sqlCommand.Parameters.AddWithValue("@TournamentId", ballStats.TournamentId == 0 ? DBNull.Value : (object)ballStats.TournamentId);
                    sqlCommand.Parameters.AddWithValue("@Inning", ballStats.Inning == 0 ? DBNull.Value : (object)ballStats.Inning);
                    sqlCommand.Parameters.AddWithValue("@OnStrikePlayerId", ballStats.OnStrike == 0 ? DBNull.Value : (object)ballStats.OnStrike);
                    sqlCommand.Parameters.AddWithValue("@OnNonStrikePlayerId", ballStats.OnNonStrike == 0 ? DBNull.Value : (object)ballStats.OnNonStrike);
                    sqlCommand.Parameters.AddWithValue("@BowlerId", ballStats.Bowler == 0 ? DBNull.Value : (object)ballStats.Bowler);
                    sqlCommand.Parameters.AddWithValue("@Runs", ballStats.Runs == 0 ? DBNull.Value : (object)ballStats.Runs);
                    sqlCommand.Parameters.AddWithValue("@Extras", string.IsNullOrEmpty(ballStats.Extras) ? DBNull.Value : (object)ballStats.Extras);
                    if (ballStats.Wickets == null)
                    {
                        sqlCommand.Parameters.AddWithValue("@Wicket", DBNull.Value);
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@Wicket", ballStats.Wickets);
                    }
                    if (ballStats.FielderContribution == null)
                    {
                        sqlCommand.Parameters.AddWithValue("@FielderContributionId", DBNull.Value);
                    }
                    else
                    {
                        sqlCommand.Parameters.AddWithValue("@FielderContributionId", ballStats.FielderContribution);
                    }
                    sqlCommand.Parameters.AddWithValue("@Commentatory", string.IsNullOrEmpty(ballStats.Commentatory) ? DBNull.Value : (object)ballStats.Commentatory);

                    try
                    {
                        sqlConnection.Open();
                        int result = sqlCommand.ExecuteNonQuery();
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
