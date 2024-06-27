using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.VO.Enums;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "JerseyNumber must be greater than 0.")]
        [Display(Name="Jersey Number")]
        public int JerseyNumber { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name="Player Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Range(18,40,ErrorMessage ="Age Must be between 18 - 40")]
        [Required]
        public int Age { get; set; }
        [Display(Name="Birth Place")]
        public string BirthPlace { get; set; }
        [Required]
        [Display(Name="Country")]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Nationality { get; set; }  
        [Display(Name="Player Type")]
        public int PlayerTypeId { get; set; }
        public PlayerType PlayerType { get; set; } 

        [Display(Name="Playes as captain")]
        public bool? IsCaptain { get; set; }
        [Display(Name="Total matches played")]
        public int? MatchesPlayed { get; set; }
        [Display(Name="Total runs scored")]
        public int? RunsScored { get; set; }
        [Display(Name="Total Wicket Taken")]
        public int? WicketTaken { get; set; }
        [Display(Name="Batting Average")]
        public double? BattingAverage { get; set; }
        [Display(Name="Bowling Average")]
        public double? BowlingAverage { get; set; }
        [Display(Name="Total Centuries")]
        public int? Centuries { get; set; }
        [Display(Name="Total Half Centuries")]
        public int? HalfCenturies { get; set; }
        [Display(Name="Debut Date")]
        public DateTime? DebutDate { get; set; }
        [Display(Name="ICC Ranking")]
        public int? ICCRanking { get; set; }
        [Display(Name="Picture")]
        public string Picture { get; set; }





        // trash
        public int BallPlayed { get; set; }
        public int Sixes { get; set; }
        public int Fours { get; set; }
        public int BallThrown { get; set; }

        // -------------
        public bool IsOut { get; set; }
        public string OutInfo { get; set; }
        public double StrikeRate { get; set; }
    }
}
