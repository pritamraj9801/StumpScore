using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class MatchService
    {
        private readonly MatchRepository _matchRepository;
        public MatchService(string connectionString)
        {
            _matchRepository = new MatchRepository(connectionString);
        }
        public bool Add(Matches match)
        {
            return _matchRepository.Add(match);
        }
        public List<Matches> GetMatchForTournament(int tournamentId)
        {
            return _matchRepository.GetMatchForTournament(tournamentId);
        }
        public Matches GetMatch(int matchId)
        {
            return _matchRepository.GetMatch(matchId);
        }
        public List<Matches> GetAll()
        {
            return _matchRepository.GetAll();
        }
    }
}
