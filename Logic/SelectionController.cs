using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class SelectionController
    {
        public List<RouteNumber> RouteNumberList;
        public Selection Selection;
        public List<RouteNumber> SortedRouteNumberList;
        readonly ListContainer _listContainer = ListContainer.GetInstance();

        public SelectionController()
        {
            RouteNumberList = new List<RouteNumber>();
            Selection = new Selection();
        }

        public void SelectWinners()
        {
            //SortRouteNumberList(listContainer.routeNumberList);
            //selection.CalculateOperationPriceDifferenceForOffers(sortedRouteNumberList);

            List<Offer> offersToAssign = new List<Offer>();
            for (int i = 0; i < SortedRouteNumberList.Count(); i++)
            {
                List<Offer> toAddToAssign = Selection.FindWinner(SortedRouteNumberList[i]);
                offersToAssign.AddRange(toAddToAssign);
            }
            List<Offer> offersThatAreIneligible = Selection.AssignWinners(offersToAssign, SortedRouteNumberList);

            bool allRouteNumberHaveWinner = DoAllRouteNumbersHaveWinner(offersThatAreIneligible);
            if (allRouteNumberHaveWinner)
            {
                Selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList(), SortedRouteNumberList);
                Selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                List<Offer> winningOffers = CreateWinnerList();
                foreach (Offer offer in winningOffers)
                {
                    _listContainer.OutputList.Add(offer);
                }
            }
            else
            {
                ContinueUntilAllRouteNumbersHaveWinner(offersThatAreIneligible);
            }
        }

        private void ContinueUntilAllRouteNumbersHaveWinner(List<Offer> offersThatAreIneligible)
        {
            while (true)
            {
                List<Offer> offersThatHaveBeenMarkedIneligible = offersThatAreIneligible;
                List<Offer> offersToAssign = new List<Offer>();

                foreach (Offer offer in offersThatHaveBeenMarkedIneligible)
                {
                    foreach (RouteNumber routeNumber in SortedRouteNumberList)
                    {
                        if (routeNumber.RouteId != offer.RouteId) continue;
                        List<Offer> offersToAssignToContractor = Selection.FindWinner(routeNumber);
                        offersToAssign.AddRange(offersToAssignToContractor);
                    }
                }
                offersThatHaveBeenMarkedIneligible = Selection.AssignWinners(offersToAssign, SortedRouteNumberList);
                bool allRouteNumberHaveWinner = DoAllRouteNumbersHaveWinner(offersThatHaveBeenMarkedIneligible);
                if (allRouteNumberHaveWinner)
                {
                    Selection.CheckIfContractorHasWonTooManyRouteNumbers(CreateWinnerList(), SortedRouteNumberList);
                    Selection.CheckForMultipleWinnersForEachRouteNumber(CreateWinnerList());
                    foreach (Offer offer in CreateWinnerList())
                    {
                        _listContainer.OutputList.Add(offer);
                    }
                } // Sidste punkt
                else
                {
                    offersThatAreIneligible = offersThatHaveBeenMarkedIneligible;
                    continue;
                }
                break;
            }
        }

        public List<Offer> CreateWinnerList()
        {
            List<Offer> winningOffers = new List<Offer>();

            foreach (ContractorOld c in _listContainer.ContractorList)
            {
                winningOffers.AddRange(c.WinningOffers);
            }
            return winningOffers;
        }
        private static bool DoAllRouteNumbersHaveWinner(IReadOnlyCollection<Offer> offersThatAreIneligible)
        {
            return offersThatAreIneligible.Count == 0;
        }
    }
}

