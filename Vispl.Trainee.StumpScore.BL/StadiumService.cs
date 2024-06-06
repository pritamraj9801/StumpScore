using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class StadiumService
    {
        private readonly StadiumRepository _stadiumRepository;
        public StadiumService(string connectionString)
        {
            _stadiumRepository = new StadiumRepository(connectionString);
        }
        public List<Stadium> GetAll()
        {
            return _stadiumRepository.GetAll();
        }
        public Stadium Get(int stadiumId)
        {
            return _stadiumRepository.Get(stadiumId);
        }
    }
}
