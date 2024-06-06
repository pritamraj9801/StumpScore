using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class MatchFormatService
    {
        private readonly MatchFormatRepository _matchFormatRepository;
        public MatchFormatService(string connectionString)
        {
            _matchFormatRepository = new MatchFormatRepository(connectionString);
        }
        public List<MatchFormat> GetAll()
        {
            return _matchFormatRepository.GetAll();
        }
    }
}
