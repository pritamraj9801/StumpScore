using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.StumpScore.BL;
using Vispl.Trainee.StumpScore.VO.Models;

namespace StumpScore.Areas.Admin.Controllers
{
    public class TeamController : Controller
    {
        private readonly PlayerService _playerService;
        private readonly TeamService _teamService;
        private readonly TournamentService _tournamentService;
        private readonly PlayerMatchService _playerMatchService;
        public TeamController()
        {
            _playerService = new PlayerService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _teamService = new TeamService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _tournamentService = new TournamentService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _playerMatchService = new PlayerMatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }
        // ------------------ Team main page dashboard
        public ActionResult Index()
        {
            List<Team> teams = _teamService.GetAll();
            foreach(var team in teams)
            {
                team.Captain = _playerService.GetAll().Where(p => p.Id == team.CaptainId).FirstOrDefault();
                team.ViceCaptain = _playerService.GetAll().Where(p => p.Id == team.ViceCaptainId).FirstOrDefault();
                team.WicketKipper = _playerService.GetAll().Where(p => p.Id == team.WicketKipperId).FirstOrDefault();
                team.Tournament = _tournamentService.GetAll().Where(t => t.Id == team.TournamentId).FirstOrDefault();
            }
            return View(teams);
        }
        // ------------------ Team create [HttpGet]
        public ActionResult Create()
        {
           
            List<Player> players = _playerService.GetAll();
            List<SelectListItem> playersList = players.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();
            ViewBag.Players = players;
            List<Tournament> tournaments = _tournamentService.GetAll();
            List<SelectListItem> tournamentsList = tournaments.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();
            ViewBag.Tournaments = tournamentsList;
            return View();
        }
        // ------------------ Team create [HttpPost]
        [HttpPost]
        public ActionResult Create(Team team, HttpPostedFileBase file, int[] selectedPlayers)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Teams"), fileName);
                    file.SaveAs(path);
                    team.TeamIcon = Path.Combine("/Content/Images/Teams", fileName);
                }
                int teamId = _teamService.Add(team);
                if (teamId>0)
                {
                    _playerMatchService.Add(selectedPlayers, team.TournamentId, teamId);
                    return RedirectToAction("Index");
                }
                else
                {
                    if (team.WicketKipperId == 0)
                    {
                        ModelState.AddModelError("WicketKipper", "Wicket kippetr is required");
                    }
                    if (team.TournamentId == 0)
                    {
                        ModelState.AddModelError("Tournament", "Wicket kipper is required");
                    }
                    List<Player> players = _playerService.GetAll();
                    List<SelectListItem> playersList = players.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToList();
                    ViewBag.Players = players;
                    List<Tournament> tournaments = _tournamentService.GetAll();
                    List<SelectListItem> tournamentsList = tournaments.Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    }).ToList();
                    ViewBag.Tournaments = tournamentsList;
                    return View(team);
                }

            }
            else
            {
                if (team.WicketKipperId == 0)
                {
                    ModelState.AddModelError("WicketKipperId", "Wicket kipper is required");
                }
                List<Player> players = _playerService.GetAll();
                List<SelectListItem> playersList = players.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
                ViewBag.Players = players;
                List<Tournament> tournaments = _tournamentService.GetAll();
                List<SelectListItem> tournamentsList = tournaments.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();
                ViewBag.Tournaments = tournamentsList;
                return View(team);
            }
        }
    }
}