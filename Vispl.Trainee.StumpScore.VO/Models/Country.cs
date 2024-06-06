using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vispl.Trainee.StumpScore.VO.Models
{
    public class Country
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string CountryName { get; set; }
        public string CountryFlag { get; set; }
    }
}
