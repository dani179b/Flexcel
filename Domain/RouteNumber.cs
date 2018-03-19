using System.Collections.Generic;

namespace Domain
{
    public class RouteNumber
    {
        public List<Offer> offers;

        public int RouteID { get; set; }
        public int RequiredVehicleType { get; set; }
        
        public RouteNumber()
        {
            offers = new List<Offer>();
        }
        public RouteNumber(int routeID, int requiredVehicleType) : this()
        {          
            this.RouteID = routeID;
            this.RequiredVehicleType = requiredVehicleType;
        }
    }
}
