using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class Selection
    {
        private readonly List<Offer> _winningOffers = new List<Offer>();
        private readonly ListContainer _listContainer = ListContainer.GetInstance();

        public void CalculateOperationPriceDifferenceForOffers(List<RouteNumber> sortedRouteNumberList)
        {
            const int lastOptionValue = int.MaxValue;
            foreach (RouteNumber routeNumber in sortedRouteNumberList)
            {
                switch (routeNumber.offers.Count)
                {
                    case 0:
                        throw new Exception("Der er ingen bud på garantivognsnummer " + routeNumber.RouteId);
                    case 1:
                        routeNumber.offers[0].DifferenceToNextOffer = lastOptionValue;
                        break;
                    case 2:
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
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
                        break;
                    default:
                        for (int i = 0; i < (routeNumber.offers.Count()) - 1; i++)
                        {
                            float difference = 0;
                            int j = i + 1;
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            if (routeNumber.offers[i].OperationPrice != routeNumber.offers[(routeNumber.offers.Count()) - 1].OperationPrice)
                            {
                                // ReSharper disable once CompareOfFloatsByEqualityOperator
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
                        break;
                }
            }
        }
        public void CheckIfContractorHasWonTooManyRouteNumbers(List<Offer> offersToCheck, List<RouteNumber> sortedRouteNumberList)
        {
            List<ContractorOld> contractorsToCheck = new List<ContractorOld>();
            foreach (Offer offer in offersToCheck)
            {
                foreach (ContractorOld contractor in _listContainer.ContractorList)
                {
                    if (contractor.UserId.Equals(offer.UserId))
                    {
                        if (!contractorsToCheck.Any(obj => obj.UserId.Equals(contractor.UserId)))
                        {
                            contractorsToCheck.Add(contractor);
                        }
                    }
                }
            }
            foreach (ContractorOld contractor in contractorsToCheck)
            {
                List<Offer> offers = contractor.CompareNumberOfWonOffersAgainstVehicles();
                if (offers.Count <= 0) continue;
                foreach (Offer offer in contractor.CompareNumberOfWonOffersAgainstVehicles())
                {
                    _listContainer.ConflictList.Add(offer);
                }
                throw new Exception("Denne entreprenør har vundet flere garantivognsnumre, end de har biler til.  Der kan ikke vælges imellem dem, da de har samme prisforskel ned til næste bud. Prioriter venligst buddene i den relevante fil i kolonnen Entreprenør Prioritet");
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
                // ReSharper disable once CompareOfFloatsByEqualityOperator
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
                _winningOffers.Add(listOfPriotizedOffers[0]);
            }
            else
            {
                foreach (Offer offer in offersWithLowestPrice)
                {
                    _winningOffers.Add(offer);
                }
            }
            return _winningOffers;
        }
        public List<Offer> AssignWinners(List<Offer> offersToAssign, List<RouteNumber> sortedRouteNumberList)
        {
            List<ContractorOld> contractorsToCheck = new List<ContractorOld>();
            List<Offer> ineligibleOffersAllContractors = new List<Offer>();

            foreach (Offer offer in offersToAssign)
            {
                if (!offer.IsEligible) continue;
                _listContainer.ContractorList.Find(x => x.UserId == offer.UserId).AddWonOffer(offer);
                contractorsToCheck.Add(offer.Contractor);
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
                                _listContainer.ConflictList.Add(offer);
                            }
                        }
                        throw new Exception("Dette garantivognsnummer har flere mulige vindere. Der kan ikke vælges mellem dem, da de har samme prisforskel ned til næste bud. Prioriter venligst buddene i den relevante fil i kolonnen Garantivognsnummer Prioritet.");
                    }
                }
            }
        }
    }
}
