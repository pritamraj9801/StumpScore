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
        public string Name { get; set; }
        [Display(Name="Short Name")]
        public string ShortName { get; set; }
        public List<Player> Players { get; set; }
        public int CaptainId { get; set; }
        public Player Captain { get; set; }

        public int ViceCaptainId { get; set; }
        public Player ViceCaptain { get; set; }

        public int WicketKipperId { get; set; }
        public Player WicketKipper { get; set; }
        [Display(Name="Team Owner")]
        public string TeamOwner { get; set; }
        [Display(Name="Team Logo")]
        public string TeamIcon { get; set; }
        [Display(Name ="Tournament")]
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

    }
}
