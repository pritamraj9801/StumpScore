using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class MatchDetailedVM
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public DateTime MatchStart { get; set; }
        public DateTime MatchEnd { get; set; }
        public Stadium Stadium { get; set; }
    }
}
