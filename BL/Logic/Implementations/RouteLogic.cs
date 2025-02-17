﻿using System;
using System.Collections.Generic;
using System.Linq;
using TrainBooking.BL.Logic.Interfaces;
using TrainBooking.DAL.Entities;
using TrainBooking.DAL.Repositories.Interfaces;

namespace TrainBooking.BL.Logic.Implementations
{
    public class RouteLogic : IRouteLogic
    {
        private readonly IRouteRepository _repository;

        public RouteLogic(IRouteRepository repository)
        {
            _repository = repository;
        }


        //todo: check if it is need
        public List<Route> GetRoutesList()
        {
            List<Route> routes = _repository.GetRoutes().ToList();
            return routes;
        }

        public Route GetRouteById(int id)
        {
            Route route = _repository.GetRouteById(id);
            return route;
        }

        public void DeleteRouteById(int id)
        {
            Route route = GetRouteById(id);
            _repository.DeleteRoute(route);
        }

        public void AddRoute(Route route)
        {
            _repository.AddRoute(route);
        }

        public void DeleteRoute(Route route)
        {
            _repository.DeleteRoute(route);
        }

        public void EditRoute(Route route)
        {
            _repository.EditRoute(route);
        }

        public List<Route> RouteSearch(string startingStationName, string lastStationName, DateTime? startingDateTime)
        {
            #region old code
            ////Edit FindRoute code
            ////List<Route> routes = (_repository.GetRoutes().Where(r => r.StartingStationRoute == startingStaion &&
            ////    r.LastStationRoute == lastStaion)).ToList(); //.Select(r => r))

            //var routes = GetRoutesList();

            //if (startingStationName != "")
            //{
            //    routes = routes.Where(r => r.StartingStationRoute.Station.Name.Contains(startingStationName)).ToList();
            //}

            //if (lastStationName != "")
            //{
            //    //routes = routes.Where(r => r.LastStationRoute.Station.Name.Contains(lastStationName)).ToList();
            //    routes = routes.Where(r => (r.LastStationRoute.Station.Name.Contains(lastStationName)) ||
            //                         (r.WayStations.Any(w => w.Station.Name.Contains(lastStationName)))).ToList();

            //    //routes = routes.Where(r => r.WayStations.Any(w => w.Station.Name.Contains(lastStationName))).ToList();
            //}

            ////if (startingDateTime != null)
            ////{
            ////    routes = routes.Where(r => r.LastStationRoute.Station.Name.Contains(lastStationName)).ToList();
            ////}
            #endregion

            List<Route> routes = null;

            startingStationName = startingStationName.ToLower();
            lastStationName = lastStationName.ToLower();

            if (startingDateTime.HasValue)
            {
                routes = _repository.GetRoutesByDepatureDate(startingDateTime).ToList();

                if (!string.IsNullOrWhiteSpace(startingStationName))
                {
                    List<Route> newRoutes =
                        _repository.GetRoutesByDateAndStationName(startingDateTime, startingStationName).ToList();

                    routes.AddRange(newRoutes);
                }
            }

            if (!string.IsNullOrWhiteSpace(startingStationName))
            {
                routes = _repository.GetRoutesWithStartStationName(startingStationName).ToList();
            }

            if (!string.IsNullOrWhiteSpace(lastStationName))
            {
                routes = _repository.GetRoutesWithLastStationName(lastStationName).ToList();
            }


            //TODO: wach and remake or remove
            if (!string.IsNullOrWhiteSpace(startingStationName) && !string.IsNullOrWhiteSpace(lastStationName))
            {
                #region first version
                //List<Route> newRoutes = new List<Route>();

                //foreach (Route route in routes)
                //{
                //    int startIndex =
                //           route.WayStations.ToList().FindIndex(w => w.Station.Name.ToLower().StartsWith(startingStationName));

                //    int lastIndex =
                //           route.WayStations.ToList().FindIndex(w => w.Station.Name.ToLower().StartsWith(lastStationName));

                //    if (startIndex < lastIndex)
                //    {
                //        newRoutes.Add(route);
                //    }
                //}
                #endregion
                //routes = (from route in routes
                //          let startIndex = route.WayStations.ToList()
                //              .FindIndex(w => w.Station.Name.ToLower().StartsWith(startingStationName))
                //          let lastIndex = route.WayStations.ToList()
                //             .FindIndex(w => w.Station.Name.ToLower().StartsWith(lastStationName))
                //          where startIndex < lastIndex
                //          select route).ToList();
            }

            return routes;
        }

        public int GetEmptyPlacesCount(Route route, List<Ticket> tickets)
        {
            //emptyPlacesCount = all places count - buzy places count 
            int emptyPlacesCount = route.Wagons.Select(w => w.WagonType.NumberOfPlaces).Sum() -
                                   tickets.Where(t => t.Wagon.Route.Id == route.Id).Select(t => t.PlaceNumber).Count();
            return emptyPlacesCount;
        }
    }
}
