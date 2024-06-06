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
    public class TeamRepository
    {
        public readonly string _connectionString;
        public TeamRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Team> GetAll()
        {
            List<Team> teams = new List<Team>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = "select * from Team";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        Team team = new Team 
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Name = row["Name"].ToString(),
                            ShortName = row["Shortname"].ToString(),
                            TeamOwner = row["TeamOwner"].ToString(),
                            TeamIcon = row["TeamIcon"].ToString(),
                            CaptainId = Convert.ToInt32(row["CaptainId"]),
                            ViceCaptainId = Convert.ToInt32(row["ViceCaptainId"]),
                            WicketKipperId = Convert.ToInt32(row["WicketKipperId"]),
                            TournamentId = Convert.ToInt32(row["TournamentId"])
                        };
                        teams.Add(team);
                    }
                }
            }
            return teams;
        }
        public int Add(Team team)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string teamQuery = @"INSERT INTO Team (Name, Shortname, TeamOwner, TeamIcon, CaptainId, ViceCaptainId, WicketKipperId, TournamentId)
                             VALUES (@Name, @Shortname, @TeamOwner, @TeamIcon, @CaptainId, @ViceCaptainId, @WicketKipperId, @TournamentId);
                             SELECT SCOPE_IDENTITY();"; // Retrieve the newly inserted team's ID

                using (SqlCommand teamCommand = new SqlCommand(teamQuery, connection))
                {
                    teamCommand.Parameters.AddWithValue("@Name", team.Name);
                    teamCommand.Parameters.AddWithValue("@Shortname", team.ShortName);
                    teamCommand.Parameters.AddWithValue("@TeamOwner", team.TeamOwner);
                    teamCommand.Parameters.AddWithValue("@TeamIcon", team.TeamIcon);
                    teamCommand.Parameters.AddWithValue("@CaptainId", team.CaptainId);
                    teamCommand.Parameters.AddWithValue("@ViceCaptainId", team.ViceCaptainId);
                    teamCommand.Parameters.AddWithValue("@WicketKipperId", team.WicketKipperId);
                    teamCommand.Parameters.AddWithValue("@TournamentId", team.TournamentId);

                    try
                    {
                        connection.Open();
                        // ExecuteScalar to get the newly inserted team's ID
                        int teamId = Convert.ToInt32(teamCommand.ExecuteScalar());
                        return teamId;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as needed
                        Console.WriteLine(ex.Message);
                        return -1; // Return -1 to indicate failure
                    }
                }
            }
        }

        public Team Get(int id)
        {
            Team team = new Team();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string query = $"select * from team where Id = {id};";
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset);
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        team.Id = Convert.ToInt32(row["Id"]);
                        team.Name = row["Name"].ToString();
                        team.ShortName = row["ShortName"].ToString();
                        team.TeamOwner = row["TeamOwner"].ToString();
                        team.TeamIcon = row["TeamIcon"].ToString();
                        team.CaptainId = Convert.ToInt32(row["CaptainId"]);
                        team.ViceCaptainId = Convert.ToInt32(row["ViceCaptainId"]);
                        team.WicketKipperId = Convert.ToInt32(row["WicketKipperId"]);
                    }
                }
            }
            return team;
        }
    }
}
