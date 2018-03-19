using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class SelectionController
    {
        public List<RouteNumber> routeNumberList;
        public Selection selection;
        public List<RouteNumber> sortedRouteNumberList;
        ListContainer listContainer = ListContainer.GetInstance();

        public SelectionController()
        {
            routeNumberList = new List<RouteNumber>();
            selection = new Selection();
        }

        private void SortRouteNumberList(List<RouteNumber> routeNumberList)
        {
            sortedRouteNumberList = routeNumberList.OrderBy(x => x.RouteID).ToList();
            foreach (RouteNumber routeNumber in sortedRouteNumberList)
            {
                routeNumber.offers = routeNumber.offers.OrderBy(x => x.OperationPrice).ThenBy(x => x.RouteNumberPriority).ToList();
            }
        }
        public void SelectWinners()
        {
            //SortRouteNumberList(listContainer.routeNumberList);
            //selection.CalculateOperationPriceDifferenceForOffers(sortedRouteNumberList);

            List<Offer> offersToAssign = new List<Offer>();
            for (int i = 0; i < sortedRouteNumberList.Count(); i++)
            {
                List<Offer> toAddToAssign = selection.FindWinner(sortedRouteNumberList[i]);
                foreach (Offer offer in toAddToAssign)
                {
                    offersToAssign.Add(offer);
                }
            }
            List<Offer> offersThatAreIneligible = selection.AssignWinners(offersToAssign, sortedRouteNumberList);

            bool allRouteNumberHaveWinner = DoAllRouteNumbersHaveWinner(offersThatAreIneligible);
            if (allRouteNumberHaveWinner)
            {
                selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList(), sortedRouteNumberList);
                selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                List<Offer> winningOffers = CreateWinnerList();
                foreach (Offer offer in winningOffers)
                {
                    listContainer.OutputList.Add(offer);
                }
            }
            else
            {
                ContinueUntilAllRouteNumbersHaveWinner(offersThatAreIneligible);
            }
        }
        private void ContinueUntilAllRouteNumbersHaveWinner(List<Offer> offersThatAreIneligible)
        {
            List<Offer> offersThatHaveBeenMarkedIneligible = offersThatAreIneligible;
            List<Offer> offersToAssign = new List<Offer>();

            foreach (Offer offer in offersThatHaveBeenMarkedIneligible)
            {
                foreach (RouteNumber routeNumber in sortedRouteNumberList)
                {
                    if (routeNumber.RouteID == offer.RouteId)
                    {
                        List<Offer> offersToAssignToContractor = selection.FindWinner(routeNumber);
                        foreach (Offer ofr in offersToAssignToContractor)
                        {
                            offersToAssign.Add(ofr);
                        }
                    }
                }
            }
            offersThatHaveBeenMarkedIneligible = selection.AssignWinners(offersToAssign, sortedRouteNumberList);
            bool allRouteNumberHaveWinner = DoAllRouteNumbersHaveWinner(offersThatHaveBeenMarkedIneligible);
            if (allRouteNumberHaveWinner)
            {
                selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList(), sortedRouteNumberList);
                selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                foreach (Offer offer in CreateWinnerList())
                {
                    listContainer.OutputList.Add(offer);
                }
            } // Sidste punkt
            else
            {
                ContinueUntilAllRouteNumbersHaveWinner(offersThatHaveBeenMarkedIneligible);
            }
        }
        public List<Offer> CreateWinnerList()
        {
            List<Offer> winningOffers = new List<Offer>();

            foreach (ContractorOLD c in listContainer.ContractorList)
            {
                foreach (Offer o in c.WinningOffers)
                {
                    winningOffers.Add(o);
                }
            }
            return winningOffers;
        }
        private bool DoAllRouteNumbersHaveWinner(List<Offer> offersThatAreIneligible)
        {
            if (offersThatAreIneligible.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

