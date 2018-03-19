using System.Collections.Generic;

namespace Domain
{
    public sealed class ListContainer
    {
        public List<RouteNumber> RouteNumberList;
        public List<ContractorOLD> ContractorList;
        public List<Offer> OutputList;
        public List<Offer> ConflictList;
        static readonly ListContainer listContainer = new ListContainer();

        private ListContainer()
        {
            RouteNumberList = new List<RouteNumber>();
            ContractorList = new List<ContractorOLD>();
            OutputList = new List<Offer>();
            ConflictList = new List<Offer>();
        }

        public static ListContainer GetInstance()
        {
            return listContainer;
        }
        public void GetLists(List<RouteNumber> routeNumberList, List<ContractorOLD> contractorList)
        {
            RouteNumberList = routeNumberList;
            ContractorList = contractorList;
        }
    }
}
