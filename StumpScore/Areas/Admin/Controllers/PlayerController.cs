using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.StumpScore.BL;
using Vispl.Trainee.StumpScore.VO.Enums;
using Vispl.Trainee.StumpScore.VO.Models;

namespace StumpScore.Areas.Admin.Controllers
{
    public class PlayerController : Controller
    {
        private readonly PlayerService _playerService;
        private readonly CountryService _countryService;
        private readonly PlayerTypeService _playerTypeService;
        public PlayerController()
        {
            _playerService = new PlayerService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _countryService = new CountryService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
            _playerTypeService = new PlayerTypeService(ConfigurationManager.ConnectionStrings["StumpScoreDbConnectionDEV"].ConnectionString);
        }

        // ------------------ players main page dashboard
        public ActionResult Index()
        {
            List<Player> players = _playerService.GetAll();
            return View(players);
        }
        // ------------------ create player page [HttpGet]
        public ActionResult Create()
        {
            // ----- getting all countries
            List<Country> Countries = _countryService.GetAll();
            List<SelectListItem> CountriesList = Countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CountryName
            }).ToList();
            ViewBag.Countries = CountriesList;

            // ----- getting all tournamnets type
            List<PlayerType> playerTypes = _playerTypeService.GetAll();
            List<SelectListItem> playerTypeList = playerTypes.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.TypeName
            }).ToList();
            ViewBag.PlayersType = playerTypeList;
            return View();
        }
        // ------------------ create player page [HttpPost]
        [HttpPost]
        public ActionResult Create(Player player, List<HttpPostedFileBase> playerImage)
        {
            // ----- if model is valid
            if (ModelState.IsValid)
            {
                // -----storing the player picture if provided
                if (playerImage != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(playerImage[1].FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/Players"), fileName);
                    playerImage[1].SaveAs(path);
                    player.Picture = Path.Combine("/Content/Images/Players", fileName);
                }
                // ----- if player added successfully
                if (_playerService.Add(player))
                {
                    return RedirectToAction("Index");
                }
                // ----- if player is not added successfully
                else
                {
                    return View(player);
                }
            }
            // ----- if model is not valid
            else
            {
                // ----- getting all countries
                List<Country> Countries = _countryService.GetAll();
                List<SelectListItem> CountriesList = Countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CountryName
                }).ToList();
                ViewBag.Countries = CountriesList;
                // ----- getting all tournamnets type
                List<PlayerType> playerTypes = _playerTypeService.GetAll();
                List<SelectListItem> playerTypeList = playerTypes.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.TypeName
                }).ToList();
                ViewBag.PlayersType = playerTypeList;
                return View(player);
            }
        }
    }
}