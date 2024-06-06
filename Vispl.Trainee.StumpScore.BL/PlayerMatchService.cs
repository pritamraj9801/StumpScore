using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class PlayerMatchService
    {
        private readonly PlayerMatchRepository _playerMatchRepository;
        public PlayerMatchService(string connectionString)
        {
            _playerMatchRepository = new PlayerMatchRepository(connectionString);
        }
        public bool Add(int[] players, int tournamentId, int teamId)
        {
            return _playerMatchRepository.Add(players, tournamentId, teamId);   
        }
        public List<Player> GetPlayersForMatch(int teamId, int tournamentId)
        {
            return _playerMatchRepository.GetPlayersForMatch(teamId, tournamentId);
        }
    }
}
