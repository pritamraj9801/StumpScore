using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class AdminDashboardVM
    {
        public List<Tournament> Tournaments { get; set; }
        public List<Matches> Matches { get; set; }
    }
}
