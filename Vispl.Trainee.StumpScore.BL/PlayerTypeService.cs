using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Enums;

namespace Vispl.Trainee.StumpScore.BL
{
    public class PlayerTypeService
    {
        private readonly PlayerTypeRepository _playerTypeRepository;
        public PlayerTypeService(string connectionString)
        {
            _playerTypeRepository = new PlayerTypeRepository(connectionString);
        }
        public List<PlayerType> GetAll()
        {
            return _playerTypeRepository.GetAll();
        }
    }
}
