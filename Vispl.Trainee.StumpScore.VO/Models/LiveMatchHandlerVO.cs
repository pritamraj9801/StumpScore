using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class LiveMatchHandlerVO
    {
        public Matches Matches { get; set; }
        public BallStats BallStats { get; set; }

        public Inning Inning1 { get; set; }
        public Inning Inning2 { get; set; }

    }
    public class Inning
    {
        public string Extras { get; set; }
        public int ExtrasRuns { get; set; }
        public int TotalScore  { get; set; }
        public int TotalWicket { get; set; }
        public int TotalBalls { get; set; }
    }
}
