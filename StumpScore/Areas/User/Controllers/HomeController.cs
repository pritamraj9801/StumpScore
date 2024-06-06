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

        public HomeController()
        {
            _tournamentService = new TournamentService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }
        public ActionResult Index()
        {
            UserDashboardVM userDashboard = new UserDashboardVM();
            userDashboard.Tournaments = _tournamentService.GetAll();

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
    }
}