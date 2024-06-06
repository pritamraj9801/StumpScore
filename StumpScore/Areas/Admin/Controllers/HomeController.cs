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
    public class HomeController : Controller
    {
        TournamentService _tournamentService;
        MatchService _matchService;
        public HomeController()
        {
            _tournamentService = new TournamentService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _matchService = new MatchService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }
        // ------------------ landing page for admin dashboard
        public ActionResult Index()
        {
            AdminDashboardVM adminDashboardVM = new AdminDashboardVM();
            adminDashboardVM.Tournaments = _tournamentService.GetAll();
            adminDashboardVM.Matches = _matchService.GetAll();
            return View(adminDashboardVM);
        }
    }
}