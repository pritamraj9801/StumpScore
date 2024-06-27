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
    public class TournamentController : Controller
    {
        private readonly MatchFormatService _matchFormatService;
        private readonly TournamentService _tournamentService;
        public TournamentController()
        {
            _matchFormatService = new MatchFormatService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _tournamentService = new TournamentService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }
        // ------------------ Tournament Main page dashboard
        public ActionResult Index()
        {
            List<Tournament> tournaments = _tournamentService.GetAll();
            foreach (var tournament in tournaments)
            {
                tournament.MatchFormat = _matchFormatService.GetAll().Where(f => f.Id == tournament.MatchFormatId).FirstOrDefault();
            }
            return View(tournaments);
        }
        // ------------------ Tournament Create [HttpGet]
        public ActionResult Create()
        {
            //Tournament tournament = new Tournament();
            List<MatchFormat> matchFormats = _matchFormatService.GetAll();
            ViewBag.MatchFormats = matchFormats;
            //return View(tournament);
            return View();
        }
        // ------------------ Tournament Create [HttpPost]
        [HttpPost]
        public ActionResult Create(Tournament tournament)
        {
            if (tournament.MatchFormatId == 0)
            {
                ModelState.AddModelError("MatchFormatId", "Match format is required");
            }
            if (ModelState.IsValid)
            {
                if (_tournamentService.Add(tournament))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    List<MatchFormat> matchFormats = _matchFormatService.GetAll();
                    ViewBag.MatchFormats = matchFormats;
                    return View(tournament);
                }
            }
            else
            {
                List<MatchFormat> matchFormats = _matchFormatService.GetAll();
                ViewBag.MatchFormats = matchFormats;
                return View(tournament);
            }
        }
    }
}