using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class User
    {
        [Display(Name ="Email Id")]
        public string ID { get; set; }
        public string Password { get; set; }
    }
}
