using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class CountryService
    {
        private readonly CountryRepository _countryRepository;
        public CountryService(string connectionString)
        {
            _countryRepository = new CountryRepository(connectionString);
        }
        public List<Country> GetAll()
        {
            return _countryRepository.GetAll();
        }
    }
}
