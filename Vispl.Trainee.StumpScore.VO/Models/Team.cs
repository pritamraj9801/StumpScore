using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Short Name")]
        [Required]
        public string ShortName { get; set; }
        public List<Player> Players { get; set; }
        [Required]
        public int CaptainId { get; set; }
        public Player Captain { get; set; }
        [Required]
        public int ViceCaptainId { get; set; }
        public Player ViceCaptain { get; set; }
        [Display(Name="Wicket Kipper")]
        [Required]
        public int WicketKipperId { get; set; }
        public Player WicketKipper { get; set; }
        [Display(Name="Team Owner")]
        public string TeamOwner { get; set; }
        [Display(Name="Team Logo")]
        public string TeamIcon { get; set; }
        [Display(Name ="Tournament")]
        [Required]
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

    }
}
