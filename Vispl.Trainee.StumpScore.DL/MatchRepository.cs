using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.DL
{
    public class MatchRepository
    {
        private readonly string _connectionString;
        public MatchRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool Add(Matches match)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Matches (Team1Id, Team2Id, StadiumId, MatchStart, MatchEnd, TournamentId)
                             VALUES (@Team1Id, @Team2Id, @StadiumId, @MatchStart, @MatchEnd, @TournamentId);
                             SELECT SCOPE_IDENTITY();";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Team1Id", match.Team1Id);
                    sqlCommand.Parameters.AddWithValue("@Team2Id", match.Team2Id);
                    sqlCommand.Parameters.AddWithValue("@StadiumId", match.StadiumId);
                    sqlCommand.Parameters.AddWithValue("@MatchStart", match.MatchStart);
                    sqlCommand.Parameters.AddWithValue("@MatchEnd", match.MatchEnd);
                    sqlCommand.Parameters.AddWithValue("@TournamentId", match.TournamentId);
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
        public List<Matches> GetMatchForTournament(int tournamentId)
        {
            List<Matches> matchList = new List<Matches>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = @"select
Matches.Id as 'MatchId',
Matches.MatchStart,
Matches.MatchEnd,
stadium.StadiumName,
team1.Id as 'Team1Id',
team2.Id as 'Team2Id',
team1.Name as 'Team1Name',
team2.Name as 'Team2Name',
Matches.TournamentId
from Matches
left join Team as team1 on Matches.Team1Id=team1.Id
left join Team as team2 on Matches.Team2Id=team2.Id
left join stadium on Matches.StadiumId=stadium.Id
where Matches.TournamentId = " + tournamentId;
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        Matches match = new Matches();
                        match.Id = Convert.ToInt32(row["MatchId"]);
                        match.MatchStart = Convert.ToDateTime(row["MatchStart"]);
                        match.MatchEnd = Convert.ToDateTime(row["MatchEnd"]);

                        Stadium stadium = new Stadium();
                        stadium.StadiumName = row["StadiumName"].ToString();
                        match.Stadium = stadium;
                        match.Team1Id = Convert.ToInt32(row["Team1Id"]);
                        Team team1 = new Team();
                        team1.Name = row["Team1Name"].ToString();
                        match.Team1 = team1;

                        Team team2 = new Team();
                        team2.Name = row["Team2Name"].ToString();
                        match.Team2 = team2;

                        match.Team2Id = Convert.ToInt32(row["Team2Id"]);
                        match.TournamentId = Convert.ToInt32(row["TournamentId"]);
                        matchList.Add(match);
                    }
                }
            }
            return matchList;
        }
        public Matches GetMatch(int matchId)
        {
            Matches match = new Matches();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = $"select * from Matches where Id = {matchId}";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        match.Team1Id = Convert.ToInt32(row["Team1Id"]);
                        match.Team2Id = Convert.ToInt32(row["Team2Id"]);
                        match.StadiumId = Convert.ToInt32(row["StadiumId"]);
                        match.MatchStart = Convert.ToDateTime(row["MatchStart"]);
                        match.MatchEnd = Convert.ToDateTime(row["MatchEnd"]);
                    }
                }
            }
            return match;
        }
        public List<Matches> GetAll()
        {
            List<Matches> matchList = new List<Matches>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = @"select
Matches.Id as 'MatchId',
Matches.MatchStart,
Matches.MatchEnd,
stadium.StadiumName,
team1.Id as 'Team1Id',
team2.Id as 'Team2Id',
team1.Name as 'Team1Name',
team2.Name as 'Team2Name',
Matches.TournamentId
from Matches
left join Team as team1 on Matches.Team1Id=team1.Id
left join Team as team2 on Matches.Team2Id=team2.Id
left join stadium on Matches.StadiumId=stadium.Id";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset);
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        Matches match = new Matches();
                        match.Id = Convert.ToInt32(row["MatchId"]);
                        match.MatchStart = Convert.ToDateTime(row["MatchStart"]);
                        match.MatchEnd = Convert.ToDateTime(row["MatchEnd"]);

                        Stadium stadium = new Stadium();
                        stadium.StadiumName = row["StadiumName"].ToString();
                        match.Stadium = stadium;
                        match.Team1Id = Convert.ToInt32(row["Team1Id"]);
                        Team team1 = new Team();
                        team1.Name = row["Team1Name"].ToString();
                        match.Team1 = team1;

                        Team team2 = new Team();
                        team2.Name = row["Team2Name"].ToString();
                        match.Team2 = team2;

                        match.Team2Id = Convert.ToInt32(row["Team2Id"]);
                        match.TournamentId = Convert.ToInt32(row["TournamentId"]);
                        matchList.Add(match);
                    }
                }
            }
            return matchList;
        }
    }
}
