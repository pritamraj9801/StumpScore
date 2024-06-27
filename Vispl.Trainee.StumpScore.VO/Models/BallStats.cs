using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class BallStats
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int TournamentId { get; set; }
        public int Inning { get; set; }
        public int OnStrike { get; set; }
        public int OnNonStrike { get; set; }
        public int Bowler { get; set; }
        public int Runs { get; set; }
        public string Extras { get; set; }
        public string Wickets { get; set; }
        public int? FielderContribution { get; set; }

        public string Commentatory { get; set; }

    }
}
