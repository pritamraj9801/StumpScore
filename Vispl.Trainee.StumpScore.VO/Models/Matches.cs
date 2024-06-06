using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class Matches
    {
        public int Id { get; set; }
        [Display(Name ="Team 1")]
        public int Team1Id { get; set; }
        public Team Team1 { get; set; }
        [Display(Name ="Team 2")]
        public int Team2Id { get; set;}
        public Team Team2 { get; set; }
        [Display(Name ="Stadium")]
        public int StadiumId { get; set; }
        public Stadium Stadium { get; set; }
        [Display(Name ="Match Start")]
        public DateTime MatchStart { get; set; }
        [Display(Name ="Match End")]
        public DateTime MatchEnd { get; set; }
        [Display(Name ="Tournament")]
        public int TournamentId { get; set; }
    }
}
