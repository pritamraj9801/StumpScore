using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        [Display(Name = "Tournament Name")]
        [Required]
        public string Name { get; set; }
        [Display(Name="Short Name")]
        [Required]
        public string ShortName { get; set; }
        [Display(Name = "Starting Date")]
        public DateTime StartingDate { get; set; }
        [Display(Name = "Ending Date")]
        public DateTime EndingDate { get; set; }
        [Display(Name = "Match Format")]
        public int MatchFormatId { get; set; }
        [Display(Name = "Match Format")]
        public MatchFormat MatchFormat { get; set; }

        [Display(Name = "Tournament Logo")]
        public string TournamentIcon { get; set; }
    }
}
