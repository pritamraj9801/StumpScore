using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.StumpScore.BL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace StumpScore.Areas.Admin.Controllers
{
    public class MatchController : Controller
    {
        private readonly TeamService _teamService;
        private readonly MatchService _matchService;
        private readonly StadiumService _stadiumService;
        private readonly TournamentService _tournamentService;
        private readonly PlayerMatchService _playerMatchService;
        public MatchController()
        {
            _teamService = new TeamService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _matchService = new MatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _stadiumService = new StadiumService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _tournamentService = new TournamentService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _playerMatchService = new PlayerMatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }
        // ------------------ match page dashboard
        public ActionResult Index()
        {
            List<Tournament> tournaments = _tournamentService.GetAll();
            return View(tournaments);
        }
        // ------------------ match craete [HttpGet]
        public ActionResult Create()
        {
            Matches match = new Matches();
            List<Team> teams = _teamService.GetAll();
            List<SelectListItem> teamsList = teams.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
            }).ToList();
            ViewBag.Teams = teamsList;
            List<Stadium> stadiums = _stadiumService.GetAll();
            ViewBag.Stadiums = stadiums;
            List<SelectListItem> tournaments = _tournamentService.GetAll().Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
            }).ToList();
            ViewBag.Tournaments = tournaments;
            return View(match);
        }
        // ------------------ match create [HttpPost]
        [HttpPost]
        public ActionResult Create(Matches match)
        {
            if (_matchService.Add(match))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // ---------- getting all tournaments
        public ActionResult MatchesForTournament(int tournamentId)
        {
            List<Matches> matchList = _matchService.GetMatchForTournament(tournamentId);
            return PartialView("_Matches", matchList);
        }
        // -------- getting detailed view of a match
        public ActionResult MatchDetailedSummary(int matchId, int tournamentId)
        {
            Matches match = _matchService.GetMatch(matchId);
            MatchDetailedVM matchDetailedVM = new MatchDetailedVM();

            Team team1 = _teamService.Get(match.Team1Id);
            team1.Players = _playerMatchService.GetPlayersForMatch(match.Team1Id, tournamentId);

            Team team2 = _teamService.Get(match.Team2Id);
            team2.Players = _playerMatchService.GetPlayersForMatch(match.Team2Id,tournamentId);

            matchDetailedVM.Team1 = team1;
            matchDetailedVM.Team2 = team2;
            matchDetailedVM.MatchStart = match.MatchStart;
            matchDetailedVM.MatchEnd = match.MatchEnd;
            matchDetailedVM.Stadium = _stadiumService.Get(match.StadiumId);
            return View(matchDetailedVM);
        }
    }
}