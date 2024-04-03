using hotel_bookings.Areas.Admin.Data;
using hotel_bookings.Areas.Admin.Service;
using hotel_bookings.Repositories;
using hotel_bookings.Services;
using Ninject.Modules;
namespace hotel_bookings
{
    public class MyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRoomService>().To<RoomService>();
            Bind<IOrderRoomService>().To<OrderRoomService>();

            Bind<IRoomServices>().To<RoomServices>();

        }
    }
}