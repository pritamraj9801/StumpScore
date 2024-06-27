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
        [Display(Name = "Team 1")]
        [Required]
        public int Team1Id { get; set; }
        public Team Team1 { get; set; }
        [Display(Name = "Team 2")]
        [Required]
        public int Team2Id { get; set; }
        public Team Team2 { get; set; }
        [Display(Name = "Stadium")]
        [Required]
        public int StadiumId { get; set; }
        public Stadium Stadium { get; set; }
        [Required]
        [Display(Name = "Match Start")]
        public DateTime MatchStart { get; set; }

        [Display(Name = "Match End")]
        [Required]
        public DateTime MatchEnd { get; set; }
        [Display(Name = "Tournament")]
        [Required]
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
        public int? TossWonBy { get; set; }
        public string OptionChoosen { get; set; }
    }
}
