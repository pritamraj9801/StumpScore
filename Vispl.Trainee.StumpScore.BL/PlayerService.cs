using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vispl.Trainee.StumpScore.DL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace Vispl.Trainee.StumpScore.BL
{
    public class PlayerService
    {
        private readonly PlayerRepository _playerRepository;
        public PlayerService(string _connectionString)
        {
            _playerRepository = new PlayerRepository(_connectionString);
        }

        public bool Add(Player player)
        {
            return _playerRepository.Add(player);
        }
        public List<Player> GetAll()
        {
            return _playerRepository.GetAll();
        }
    }
}
