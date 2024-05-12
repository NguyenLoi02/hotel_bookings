using hotel_bookings.Models.Data;
using hotel_bookings.Models.Service;
using Ninject.Modules;
namespace hotel_bookings
{
    public class MyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRoomService>().To<RoomService>();
            Bind<INewsService>().To<NewsService>();
            Bind<IVnPayService>().To<VnPayService>();



        }
    }
}