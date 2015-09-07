﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainBooking.BL;
using TrainBooking.BL.Logic;
using TrainBooking.DAL;
using TrainBooking.DAL.Entities;
using TrainBooking.Models;

namespace TrainBooking.Controllers
{
    public class StationController : Controller
    {
        StationLogic logic = new StationLogic();

        //IStationLogic logic = new StationLogic();

        List<StationViewModel> stationViews = new List<StationViewModel>();

        public ActionResult Index()
        {
            List<Station> stations = logic.GetStationsList();

            stationViews = stations.Select(x => new StationViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ArrivalDate = x.ArrivalDate,
                    ArrivalTime = x.ArrivalTime,
                    DepatureDate = x.DepatureDate,
                    DepatureTime = x.DepatureTime
                }).ToList();

            return View(stationViews);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Station station)
        {
            try
            {
                logic.AddStation(station);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            //
            // как сохрнить дату при редактировании сущности? поиграть с форматами выведения дат
            //
            Station station = logic.GetStationById(id);
            return View(station);
        }

        [HttpPost]
        public ActionResult Edit(Station station)
        {
            try
            {
                logic.EditStation(station);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            Station station = logic.GetStationById(id);
            return View(station);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                logic.DeleteStationById(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
