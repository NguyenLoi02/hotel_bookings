using hotel_bookings.Areas.Admin.Data;
using hotel_bookings.Areas.Admin.Service;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel_bookings
{
    public class MyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRoomService>().To<RoomService>();
        }
    }
}