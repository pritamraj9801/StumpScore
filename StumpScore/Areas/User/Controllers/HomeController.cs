using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.StumpScore.BL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace StumpScore.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly TournamentService _tournamentService;
        private readonly MatchService _matchService;
        private readonly TeamService _teamService;
        private readonly PlayerMatchService _playerMatchService;

        public HomeController()
        {
            _tournamentService = new TournamentService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _matchService = new MatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _teamService = new TeamService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _playerMatchService = new PlayerMatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }
        public ActionResult Index()
        {
            UserDashboardVM userDashboard = new UserDashboardVM();
            userDashboard.Matches = _matchService.GetAll();

            return View("~/Areas/User/Views/Home/Index.cshtml", userDashboard);
            //  return View();
        }
        public ActionResult SignIn()
        {
            Vispl.Trainee.StumpScore.VO.Models.User user = new Vispl.Trainee.StumpScore.VO.Models.User();
            return View("~/Areas/User/Views/Home/SignIn.cshtml", user);
            //return View(user);
        }
        [HttpPost]
        public ActionResult SignIn(Vispl.Trainee.StumpScore.VO.Models.User user)
        {
            if ((user.ID != null && user.ID.ToString() == "Id") && (user.Password != null && user.Password.ToString() == "Password"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                ModelState.AddModelError("", "Email id or Password is Incorrect");
                return View("~/Areas/User/Views/Home/SignIn.cshtml", user);
                //return View(user);
            }
        }
        public ActionResult LiveMatchHandler(int matchId, int tournamentId)
        {           
            Matches match = _matchService.GetMatch(matchId);
            Team team1 = _teamService.Get(match.Team1Id);
            team1.Players = _playerMatchService.GetPlayersForMatch(match.Team1Id, tournamentId);
            match.Team1 = team1;
            Team team2 = _teamService.Get(match.Team2Id);
            team2.Players = _playerMatchService.GetPlayersForMatch(match.Team2Id, tournamentId);
            match.Team2 = team2;
            return View(match);
        }
    }
}