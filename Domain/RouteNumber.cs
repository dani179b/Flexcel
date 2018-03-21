using System.Collections.Generic;

namespace Domain
{
    public class RouteNumber
    {
        public List<Offer> offers;

        public int RouteId { get; set; }
        public int RequiredVehicleType { get; set; }

        public RouteNumber()
        {
            offers = new List<Offer>();
        }
        public RouteNumber(int routeId, int requiredVehicleType)
        {
            RouteId = routeId;
            RequiredVehicleType = requiredVehicleType;
        }
    }
}
