using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class BallStatsService
    {
        private readonly BallStatsRepository _ballStatsRepository;
        public BallStatsService(string _connectionString)
        {
            _ballStatsRepository = new BallStatsRepository(_connectionString);
        }
        public bool Add(BallStats ballStats)
        {
            return _ballStatsRepository.Add(ballStats);
        }
        public List<BallStats> GetAll(int matchId, int tournamentId,int inning)
        {
            return _ballStatsRepository.GetAll(matchId, tournamentId,inning);
        }
    }
}
