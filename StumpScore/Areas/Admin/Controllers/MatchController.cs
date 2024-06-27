using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly BallStatsService _ballStatsService;
        private readonly PlayerService _playerService;
        public MatchController()
        {
            _teamService = new TeamService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _matchService = new MatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _stadiumService = new StadiumService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _tournamentService = new TournamentService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _playerMatchService = new PlayerMatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _ballStatsService = new BallStatsService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _playerService = new PlayerService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }
        // ------------------ match page dashboard
        public ActionResult Index()
        {
            List<Tournament> tournaments = _tournamentService.GetAll();
            return View(tournaments);
        }
        // ------------------ match create [HttpGet]
        public ActionResult Create()
        {
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
            return View();
        }
        // ------------------ match create [HttpPost]
        [HttpPost]
        public ActionResult Create(Matches match)
        {
            if (match.StadiumId == 0)
            {
                ModelState.AddModelError("StadiumId", "Stadium is required");
            }
            if (match.Team1Id == 0)
            {
                ModelState.AddModelError("Team1Id", "Stadium is required");
            }
            if (match.Team2Id == 0)
            {
                ModelState.AddModelError("Team2Id", "Stadium is required");
            }
            if (match.TournamentId == 0)
            {
                ModelState.AddModelError("TournamentId", "Tournament is required");
            }
            if (ModelState.IsValid)
            {
                if (_matchService.Add(match))
                {
                    return RedirectToAction("Index");
                }
            }
            // --------------- if model state is not valid
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
            team2.Players = _playerMatchService.GetPlayersForMatch(match.Team2Id, tournamentId);

            matchDetailedVM.Team1 = team1;
            matchDetailedVM.Team2 = team2;
            matchDetailedVM.MatchStart = match.MatchStart;
            matchDetailedVM.MatchEnd = match.MatchEnd;
            matchDetailedVM.Stadium = _stadiumService.Get(match.StadiumId);
            return View(matchDetailedVM);
        }
        public ActionResult StartTheMatch(int matchId, int tournamentId)
        {
            Matches match = _matchService.GetMatch(matchId);
            if (match.TossWonBy > 0)
            {
                return RedirectToAction("LiveMatchHandler", new { matchId = matchId, tournamentId = tournamentId });
            }
            else
            {
                Team team1 = _teamService.Get(match.Team1Id);
                Team team2 = _teamService.Get(match.Team2Id);

                match.Team1 = team1;
                match.Team2 = team2;
                return View(match);
            }
        }
        [HttpPost]
        public ActionResult StartTheMatch(Matches match, int toss, string choosenOption)
        {
            _matchService.UpdateToss(match, toss, choosenOption);
            return RedirectToAction("LiveMatchHandler", new { matchId = match.Id, tournamentId = match.TournamentId });
        }
        public ActionResult LiveMatchHandler(int matchId, int tournamentId)
        {
            // -------------------------- creating view for the live match
            LiveMatchHandlerVO liveMatchHandlerVO = new LiveMatchHandlerVO();
            Matches match = _matchService.GetMatch(matchId);
            match.TournamentId = tournamentId;
            match.Tournament = _tournamentService.GetAll().Where(t => t.Id == match.TournamentId).FirstOrDefault();

            Team team1 = _teamService.Get(match.Team1Id);
            team1.Players = _playerMatchService.GetPlayersForMatch(match.Team1Id, tournamentId);

            Team team2 = _teamService.Get(match.Team2Id);
            team2.Players = _playerMatchService.GetPlayersForMatch(match.Team2Id, tournamentId);

            match.Team1 = team1;
            match.Team2 = team2;

            match.Stadium = _stadiumService.Get(match.StadiumId);
            liveMatchHandlerVO.Matches = match;
            // --------------- extras in inning 1
            List<BallStats> ballStats = _ballStatsService.GetAll(matchId, tournamentId, 1);
            StringBuilder extras = new StringBuilder();
            int extraTotal = 0;
            foreach (BallStats ballStat in ballStats)
            {
                if (ballStat.Extras.Length > 0)
                {
                    if (ballStat.Extras == "wide")
                    {
                        extras.Append(ballStat.Runs + "(wd)");
                    }
                    else if (ballStat.Extras == "NoBall")
                    {
                        extras.Append(ballStat.Runs + "(nb)");
                    }
                    extraTotal += ballStat.Runs;
                    extraTotal += 1;
                }
            }
            Inning inning1 = new Inning();
            inning1.Extras = extras.ToString();
            inning1.ExtrasRuns = extraTotal;
            liveMatchHandlerVO.Inning1 = inning1;

            // ------------- extras in Inning 2
            ballStats = _ballStatsService.GetAll(matchId, tournamentId, 1);
            extras.Clear();
            extraTotal = 0;
            foreach (BallStats ballStat in ballStats)
            {
                if (ballStat.Extras.Length > 0)
                {
                    if (ballStat.Extras == "wide")
                    {
                        extras.Append(ballStat.Runs + "(wd)");
                    }
                    else if (ballStat.Extras == "NoBall")
                    {
                        extras.Append(ballStat.Runs + "(nb)");
                    }
                    extraTotal += ballStat.Runs;
                    extraTotal += 1;
                }
            }
            Inning inning2 = new Inning();
            inning1.Extras = extras.ToString();
            liveMatchHandlerVO.Inning2 = inning2;

            // --------------- batsman stats for the inning
            for (int i = 0; i < liveMatchHandlerVO.Matches.Team1.Players.Count; i++)
            {
                Player player = liveMatchHandlerVO.Matches.Team1.Players[i];
                player.RunsScored = ballStats.Where(b => b.OnStrike == player.Id && b.Extras.Length == 0).Sum(b => b.Runs);
                player.BallPlayed = ballStats.Where(b => b.OnStrike == player.Id && b.Extras.Length > 0).Count();
                player.Sixes = ballStats.Where(b => b.OnStrike == player.Id && b.Runs == 6).Count();
                player.Fours = ballStats.Where(b => b.OnStrike == player.Id && b.Runs == 4).Count();
                if (player.BallPlayed > 0)
                {
                    player.StrikeRate = (double)(player.RunsScored / player.BallPlayed) * 100.0;
                }
                else
                {
                    player.StrikeRate = 0.0;
                }
                liveMatchHandlerVO.Matches.Team1.Players[i] = player;
            }
            // ---------------- outs in inning 1
            List<BallStats> outs = ballStats.Where(b => b.Wickets.Length > 0).ToList();
            for (int i = 0; i < outs.Count; i++)
            {
                Player player = liveMatchHandlerVO.Matches.Team2.Players.Where(p => p.Id == outs[i].OnStrike).FirstOrDefault();
                if (player != null)
                {
                    liveMatchHandlerVO.Matches.Team2.Players.Where(p => p.Id == outs[i].OnStrike).FirstOrDefault().IsOut = true;
                    liveMatchHandlerVO.Matches.Team2.Players.Where(p => p.Id == outs[i].OnStrike).FirstOrDefault().OutInfo = outs[i].Wickets;
                }
            }
            // ---------------- batsman stats for the inning 2
            for (int i = 0; i < liveMatchHandlerVO.Matches.Team2.Players.Count; i++)
            {
                Player player = liveMatchHandlerVO.Matches.Team2.Players[i];
                player.RunsScored = ballStats.Where(b => b.OnStrike == player.Id && b.Extras.Length == 0).Sum(b => b.Runs);
                player.BallPlayed = ballStats.Where(b => b.OnStrike == player.Id && b.Extras.Length == 0).Count();
                player.Sixes = ballStats.Where(b => b.OnStrike == player.Id && b.Runs == 6).Count();
                player.Fours = ballStats.Where(b => b.OnStrike == player.Id && b.Runs == 4).Count();
                if (player.BallPlayed > 0)
                {
                    player.StrikeRate = (double)(player.RunsScored / (double)player.BallPlayed) * 100.0;
                }
                else
                {
                    player.StrikeRate = 0.0;
                }
                liveMatchHandlerVO.Matches.Team2.Players[i] = player;
            }

            // ---------------- bowler stats for the inning 1
            for (int i = 0; i < liveMatchHandlerVO.Matches.Team1.Players.Count; i++)
            {
                Player player = liveMatchHandlerVO.Matches.Team1.Players[i];
                player.BallThrown = ballStats.Where(b => b.Bowler == player.Id && b.Extras.Length<=0).Count();
                player.WicketTaken = ballStats.Where(b => b.Bowler == player.Id && b.Wickets.Length > 0).Count();
                liveMatchHandlerVO.Matches.Team1.Players[i] = player;
            }
            // ---------------- bowler stats for the inning 2
            for (int i = 0; i < liveMatchHandlerVO.Matches.Team2.Players.Count; i++)
            {
                Player player = liveMatchHandlerVO.Matches.Team2.Players[i];
                player.BallThrown = ballStats.Where(b => b.Bowler == player.Id && b.Extras.Length<=0).Count();
                player.WicketTaken = ballStats.Where(b => b.Bowler == player.Id && b.Wickets.Length > 0).Count();
                liveMatchHandlerVO.Matches.Team2.Players[i] = player;
            }
            // -------------- retaining the last selected options for the dropdowns
            if (TempData["ballStats"] != null)
            {
                liveMatchHandlerVO.BallStats = (BallStats)TempData["ballStats"];
            }
            else
            {
                BallStats lastBallStats = new BallStats();
                BallStats tempBallStat = _ballStatsService.GetAll(matchId, tournamentId, 1).LastOrDefault();
                if (tempBallStat != null)
                {
                    lastBallStats.OnStrike = _ballStatsService.GetAll(matchId, tournamentId, 1).LastOrDefault().OnStrike;
                    lastBallStats.OnNonStrike = _ballStatsService.GetAll(matchId, tournamentId, 1).LastOrDefault().OnNonStrike;
                    lastBallStats.Bowler = _ballStatsService.GetAll(matchId, tournamentId, 1).LastOrDefault().Bowler;
                    lastBallStats.Runs = _ballStatsService.GetAll(matchId, tournamentId, 1).LastOrDefault().Runs;
                    lastBallStats.Extras = _ballStatsService.GetAll(matchId, tournamentId, 1).LastOrDefault().Extras;
                }
                liveMatchHandlerVO.BallStats = lastBallStats;
            }
            // -------------- changing the strike
            if (liveMatchHandlerVO.BallStats.Runs == 1 || liveMatchHandlerVO.BallStats.Runs == 3)
            {
                int temp = liveMatchHandlerVO.BallStats.OnStrike;
                liveMatchHandlerVO.BallStats.OnStrike = liveMatchHandlerVO.BallStats.OnNonStrike;
                liveMatchHandlerVO.BallStats.OnNonStrike = temp;
            }
            liveMatchHandlerVO.BallStats.Extras = null;
            liveMatchHandlerVO.BallStats.Runs = 0;
            liveMatchHandlerVO.BallStats.Wickets = null;

            // ---------------------------- getting UI components
            liveMatchHandlerVO.Inning1.TotalScore = ballStats.Where(b => b.Inning == 1).Sum(b => b.Runs);
            liveMatchHandlerVO.Inning1.TotalScore += liveMatchHandlerVO.Inning1.ExtrasRuns;
            liveMatchHandlerVO.Inning1.TotalWicket = ballStats.Where(b => b.Inning == 1 && b.Wickets.Length > 0).Count();
            liveMatchHandlerVO.Inning1.TotalBalls = ballStats.Where(b => b.Inning == 1 && b.Extras.Length == 0).Count();

            liveMatchHandlerVO.Inning2.TotalScore = ballStats.Where(b => b.Inning == 2).Sum(b => b.Runs);
            liveMatchHandlerVO.Inning2.TotalScore += liveMatchHandlerVO.Inning2.ExtrasRuns;
            liveMatchHandlerVO.Inning2.TotalWicket = ballStats.Where(b => b.Inning == 2 && b.Wickets.Length > 0).Count();
            liveMatchHandlerVO.Inning2.TotalWicket = ballStats.Where(b => b.Inning == 2 && b.Extras.Length == 0).Count();
            if (liveMatchHandlerVO.Inning1.TotalBalls % 6 == 0)
            {
                liveMatchHandlerVO.BallStats.Bowler = 0;
                int temp = liveMatchHandlerVO.BallStats.OnNonStrike;
                liveMatchHandlerVO.BallStats.OnNonStrike = liveMatchHandlerVO.BallStats.OnStrike;
                liveMatchHandlerVO.BallStats.OnStrike = temp;
            }
            return View(liveMatchHandlerVO);

        }
        [HttpPost]
        public ActionResult LiveMatchHandler(LiveMatchHandlerVO liveMatchHandlerVO)
        {
            liveMatchHandlerVO.BallStats.MatchId = liveMatchHandlerVO.Matches.Id;
            liveMatchHandlerVO.BallStats.TournamentId = liveMatchHandlerVO.Matches.TournamentId;
            liveMatchHandlerVO.BallStats.Inning = 1;
            if (liveMatchHandlerVO.BallStats.Wickets!=null)
            {
                liveMatchHandlerVO.BallStats.Wickets += ", " + _playerService.GetAll().Where(p => p.Id == liveMatchHandlerVO.BallStats.Bowler).FirstOrDefault().Name;
            }


            _ballStatsService.Add(liveMatchHandlerVO.BallStats);
            TempData["ballStats"] = liveMatchHandlerVO.BallStats;
            return RedirectToAction("LiveMatchHandler", new { matchId = liveMatchHandlerVO.Matches.Id, tournamentId = liveMatchHandlerVO.Matches.TournamentId });
        }
        public ActionResult GetMatches(DateTime fromDateTime, string fromTimeZone, DateTime toDateTime, string toTimeZone)
        {
            List<Matches> matches = _matchService.GetAll().Where(m => m.MatchStart > fromDateTime && m.MatchEnd < toDateTime).ToList();
            return Json(matches, JsonRequestBehavior.AllowGet);
        }
    }
}