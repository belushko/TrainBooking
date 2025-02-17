﻿using System.Collections.Generic;
using System.Linq;
using TrainBooking.DAL.Entities;
using TrainBooking.DAL.Repositories.Interfaces;

namespace TrainBooking.DAL.Repositories.Implementations
{
    public class WagonRepository : Repository, IWagonRepository
    {
        public WagonRepository(TrainBookingContext context)
            : base(context)
        {
        }

        public List<Wagon> GetWagons()
        {
            return db.Wagons.ToList();
        }

        public Wagon GetWagonById(int id)
        {
            return db.Wagons.FirstOrDefault(w => w.Id == id);
        }
    }
}
