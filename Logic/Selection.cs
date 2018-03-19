using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class Selection
    {
        List<Offer> winningOffers = new List<Offer>();
        ListContainer listContainer = ListContainer.GetInstance();

        public void CalculateOperationPriceDifferenceForOffers(List<RouteNumber> sortedRouteNumberList)
        {
            const int lastOptionValue = int.MaxValue;
            foreach (RouteNumber routeNumber in sortedRouteNumberList)
            {
                if (routeNumber.offers.Count == 0)
                {
                    throw new Exception("Der er ingen bud på garantivognsnummer " + routeNumber.RouteID);
                }
                else if (routeNumber.offers.Count == 1)
                {
                    routeNumber.offers[0].DifferenceToNextOffer = lastOptionValue;
                }
                else if (routeNumber.offers.Count == 2)
                {
                    if (routeNumber.offers[0].OperationPrice == routeNumber.offers[1].OperationPrice)
                    {
                        routeNumber.offers[0].DifferenceToNextOffer = lastOptionValue;
                        routeNumber.offers[1].DifferenceToNextOffer = lastOptionValue;
                    }
                    else
                    {
                        routeNumber.offers[0].DifferenceToNextOffer = routeNumber.offers[1].OperationPrice - routeNumber.offers[0].OperationPrice;
                        routeNumber.offers[1].DifferenceToNextOffer = lastOptionValue;
                    }
                }
                else
                {
                    for (int i = 0; i < (routeNumber.offers.Count()) - 1; i++)
                    {
                        float difference = 0;
                        int j = i + 1;
                        if (routeNumber.offers[i].OperationPrice != routeNumber.offers[(routeNumber.offers.Count()) - 1].OperationPrice)
                        {
                            while (difference == 0 && j <= (routeNumber.offers.Count()) - 1)
                            {
                                difference = routeNumber.offers[j].OperationPrice - routeNumber.offers[i].OperationPrice;
                                j++;
                            }
                        }
                        else
                        {
                            while (i < (routeNumber.offers.Count()) - 1)
                            {
                                routeNumber.offers[i].DifferenceToNextOffer = lastOptionValue;
                                i++;
                            }
                        }
                        routeNumber.offers[i].DifferenceToNextOffer = difference;
                    }
                    routeNumber.offers[(routeNumber.offers.Count()) - 1].DifferenceToNextOffer = lastOptionValue;

                }
            }
        }
        public void CheckIfContractorHasWonTooManyRouteNumbers(List<Offer> offersToCheck, List<RouteNumber> sortedRouteNumberList)
        {
            List<ContractorOLD> contractorsToCheck = new List<ContractorOLD>();
            foreach (Offer offer in offersToCheck)
            {
                foreach (ContractorOLD contractor in listContainer.ContractorList)
                {
                    if (contractor.UserID.Equals(offer.UserId))
                    {
                        if (!contractorsToCheck.Any(obj => obj.UserID.Equals(contractor.UserID)))
                        {
                            contractorsToCheck.Add(contractor);
                        }
                    }
                }
            }
            foreach (ContractorOLD contractor in contractorsToCheck)
            {
                List<Offer> offers = contractor.CompareNumberOfWonOffersAgainstVehicles();
                if (offers.Count > 0)
                {
                    foreach (Offer offer in contractor.CompareNumberOfWonOffersAgainstVehicles())
                    {
                        listContainer.ConflictList.Add(offer);
                    }
                    throw new Exception("Denne entreprenør har vundet flere garantivognsnumre, end de har biler til.  Der kan ikke vælges imellem dem, da de har samme prisforskel ned til næste bud. Prioriter venligst buddene i den relevante fil i kolonnen Entreprenør Prioritet");
                }
            }
        }
        public List<Offer> FindWinner(RouteNumber routeNumber)
        {
            List<Offer> offersWithLowestPrice = new List<Offer>();
            float lowestEligibleOperationPrice = 0;

            for (int i = 0; i < routeNumber.offers.Count(); i++)
            {
                if (routeNumber.offers[i].IsEligible)
                {
                    lowestEligibleOperationPrice = routeNumber.offers[i].OperationPrice;
                }
            }
            foreach (Offer offer in routeNumber.offers)
            {
                if (offer.IsEligible && offer.OperationPrice == lowestEligibleOperationPrice)
                {
                    offersWithLowestPrice.Add(offer);
                }
            }

            int count = 0;
            foreach (Offer offer in offersWithLowestPrice) // Checking if offers with same price are prioritized
            {
                if (offer.RouteNumberPriority != 0)
                {
                    count++;
                }
            }
            if (count != 0) //if routenumberpriority found 

            {
                List<Offer> listOfPriotizedOffers = new List<Offer>();
                foreach (Offer offer in offersWithLowestPrice)
                {
                    if (offer.RouteNumberPriority > 0)
                    {
                        listOfPriotizedOffers.Add(offer);
                    }
                }

                listOfPriotizedOffers = listOfPriotizedOffers.OrderBy(x => x.RouteNumberPriority).ToList();
                winningOffers.Add(listOfPriotizedOffers[0]);
            }
            else
            {
                foreach (Offer offer in offersWithLowestPrice)
                {
                    winningOffers.Add(offer);
                }
            }
            return winningOffers;
        }
        public List<Offer> AssignWinners(List<Offer> offersToAssign, List<RouteNumber> sortedRouteNumberList)
        {
            List<ContractorOLD> contractorsToCheck = new List<ContractorOLD>();
            List<Offer> ineligibleOffersAllContractors = new List<Offer>();

            foreach (Offer offer in offersToAssign)
            {
                if (offer.IsEligible)
                {
                    listContainer.ContractorList.Find(x => x.UserID == offer.UserId).AddWonOffer(offer);
                    contractorsToCheck.Add(offer.Contractor);
                }
            }

            for (int i = 0; i < contractorsToCheck.Count(); i++)
            {
                contractorsToCheck[i].CompareNumberOfWonOffersAgainstVehicles();
                List<Offer> ineligibleOffersOneContractor = contractorsToCheck[i].IneligibleOffers();
                ineligibleOffersAllContractors.AddRange(ineligibleOffersOneContractor);
                contractorsToCheck[i].RemoveIneligibleOffers();
            }

            return ineligibleOffersAllContractors;
        }
        public void CheckForMultipleWinnersForEachRouteNumber(List<Offer> winnerList)
        {
            for (int i = 0; i < winnerList.Count; i++)
            {
                for (int j = i + 1; j < winnerList.Count; j++)
                {
                    if (winnerList[i].RouteId == winnerList[j].RouteId)
                    {
                        foreach (Offer offer in winnerList)
                        {
                            if (offer.RouteId == winnerList[i].RouteId)
                            {
                                listContainer.ConflictList.Add(offer);
                            }
                        }
                        throw new Exception("Dette garantivognsnummer har flere mulige vindere. Der kan ikke vælges mellem dem, da de har samme prisforskel ned til næste bud. Prioriter venligst buddene i den relevante fil i kolonnen Garantivognsnummer Prioritet.");
                    }
                }
            }
        }
    }
}
