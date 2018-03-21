using System.Collections.Generic;

namespace Domain
{
    public sealed class ListContainer
    {
        public List<RouteNumber> RouteNumberList;
        public List<ContractorOld> ContractorList;
        public List<Offer> OutputList;
        public List<Offer> ConflictList;
        private static readonly ListContainer listContainer = new ListContainer();

        private ListContainer()
        {
            RouteNumberList = new List<RouteNumber>();
            ContractorList = new List<ContractorOld>();
            OutputList = new List<Offer>();
            ConflictList = new List<Offer>();
        }

        public static ListContainer GetInstance()
        {
            return listContainer;
        }
        public void GetLists(List<RouteNumber> routeNumberList, List<ContractorOld> contractorList)
        {
            RouteNumberList = routeNumberList;
            ContractorList = contractorList;
        }
    }
}
