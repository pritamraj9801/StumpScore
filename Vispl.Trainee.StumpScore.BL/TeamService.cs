using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class TeamService
    {
        private readonly TeamRepository _teamRepository;
        public TeamService(string connectionString)
        {
            _teamRepository = new TeamRepository(connectionString);
        }
        public List<Team> GetAll()
        {
            return _teamRepository.GetAll();
        }
        public int Add(Team team)
        {
            return _teamRepository.Add(team);
        }
        public Team Get(int id)
        {
            return _teamRepository.Get(id);
        }
    }
}
