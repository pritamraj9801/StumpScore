using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class TournamentService
    {
        private readonly TournamentRepository _tournamentRepository;
        public TournamentService(string connectionString)
        {
            _tournamentRepository = new TournamentRepository(connectionString);
        }
        public List<Tournament> GetAll()
        {
            return _tournamentRepository.GetAll();
        }
        public bool Add(Tournament tournament)
        {
            return _tournamentRepository.Add(tournament);
        }
    }
}
