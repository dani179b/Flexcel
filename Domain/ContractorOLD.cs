using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class ContractorOld
    {
        private int _type2;
        private int _type3;
        private int _type5;
        private int _type6;
        private int _type7;

        public List<Offer> WinningOffers;
        public List<Offer> InEligibleOffers;

        public string ReferenceNumberBasicInformationPdf { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string ManagerName { get; set; }
        public int NumberOfType2PledgedVehicles { get; set; }
        public int NumberOfType3PledgedVehicles { get; set; }
        public int NumberOfType5PledgedVehicles { get; set; }
        public int NumberOfType6PledgedVehicles { get; set; }
        public int NumberOfType7PledgedVehicles { get; set; }
        public string TryParseValueType2PledgedVehicles { get; set; }
        public string TryParseValueType3PledgedVehicles { get; set; }
        public string TryParseValueType5PledgedVehicles { get; set; }
        public string TryParseValueType6PledgedVehicles { get; set; }
        public string TryParseValueType7PledgedVehicles { get; set; }
        public int NumberOfWonType2Offers { get; private set; }
        public int NumberOfWonType3Offers { get; private set; }
        public int NumberOfWonType5Offers { get; private set; }
        public int NumberOfWonType6Offers { get; private set; }
        public int NumberOfWonType7Offers { get; private set; }

        public ContractorOld()
        {
            WinningOffers = new List<Offer>();
            InEligibleOffers = new List<Offer>();
        }
        public ContractorOld(
            string referenceNumberBasicInformationPdf, string userId, string companyName,
            string managerName, int numberOfType2PledgedVehicles, int numberOfType3PledgedVehicles, int numberOfType5PledgedVehicles,
            int numberOfType6PledgedVehicles, int numberOfType7PledgedVehicles) : this()
        {
            ReferenceNumberBasicInformationPdf = referenceNumberBasicInformationPdf;
            UserId = userId;
            CompanyName = companyName;
            ManagerName = managerName;
            NumberOfType2PledgedVehicles = numberOfType2PledgedVehicles;
            NumberOfType3PledgedVehicles = numberOfType3PledgedVehicles;
            NumberOfType5PledgedVehicles = numberOfType5PledgedVehicles;
            NumberOfType6PledgedVehicles = numberOfType6PledgedVehicles;
            NumberOfType7PledgedVehicles = numberOfType7PledgedVehicles;
        }

        public void AddWonOffer(Offer offer)
        {
            bool alreadyOnTheList = WinningOffers.Any(item => item.OfferReferenceNumber == offer.OfferReferenceNumber);
            if (!alreadyOnTheList)
            {
                WinningOffers.Add(offer);
            }
            else
            {
                foreach (Offer winOffer in WinningOffers)
                {
                    if (winOffer.OfferReferenceNumber == offer.OfferReferenceNumber)
                    {
                        winOffer.IsEligible = true;
                    }
                }
            }
        }

        public List<Offer> IneligibleOffers()
        {
            foreach (Offer offer in WinningOffers)
            {
                if (!offer.IsEligible)
                {
                    InEligibleOffers.Add(offer);
                }
            }
            return InEligibleOffers;
        }

        public void RemoveIneligibleOffers()
        {
            if (InEligibleOffers.Count > 0)
            {
                foreach (Offer offer in InEligibleOffers)
                {
                    WinningOffers.Remove(offer);
                }
            }
        }

        public List<Offer> CompareNumberOfWonOffersAgainstVehicles()
        {
            List<Offer> offersWithConflict = new List<Offer>();
            if (WinningOffers.Count > 0)
            {
                foreach (Offer offer in WinningOffers)
                {
                    if (offer.IsEligible)
                    {
                        switch (offer.RequiredVehicleType)
                        {
                            case 2:
                                _type2++;
                                break;
                            case 3:
                                _type3++;
                                break;
                            case 5:
                                _type5++;
                                break;
                            case 6:
                                _type6++;
                                break;
                            case 7:
                                _type7++;
                                break;
                        }
                    }
                }
                if (NumberOfType2PledgedVehicles > 0 && NumberOfType3PledgedVehicles > 0 && NumberOfType5PledgedVehicles > 0 && NumberOfType6PledgedVehicles > 0 && NumberOfType7PledgedVehicles > 0)
                {
                    offersWithConflict.AddRange(IfTooManyWonOffers(NumberOfType2PledgedVehicles, _type2, 2));
                    offersWithConflict.AddRange(IfTooManyWonOffers(NumberOfType3PledgedVehicles, _type3, 3));
                    offersWithConflict.AddRange(IfTooManyWonOffers(NumberOfType5PledgedVehicles, _type5, 5));
                    offersWithConflict.AddRange(IfTooManyWonOffers(NumberOfType6PledgedVehicles, _type6, 6));
                    offersWithConflict.AddRange(IfTooManyWonOffers(NumberOfType7PledgedVehicles, _type7, 7));
                }
            }
            return offersWithConflict;
        }
        private List<Offer> IfTooManyWonOffers(int numberOfPledgedVehicles, int numberOfWonOffersWithThisType, int type)
        {
            List<Offer> offersToCheck = new List<Offer>();
            List<Offer> getOffers = new List<Offer>();
            foreach (Offer winningOffer in WinningOffers)
            {
                if (winningOffer.IsEligible && winningOffer.RequiredVehicleType == type)
                {
                    offersToCheck.Add(winningOffer);
                }
            }

            if (numberOfPledgedVehicles < numberOfWonOffersWithThisType)
            {
                if (numberOfPledgedVehicles == 0) //This is done because, sometimes contractors place bids on routenumbers, they don't have the correct vehicle type for. 
                {
                    foreach (Offer offer in WinningOffers)
                    {
                        if (offer.RequiredVehicleType == type)
                        {
                            offer.IsEligible = false;
                        }
                    }
                }
                else
                {
                    getOffers = FindOptimalWins(offersToCheck, numberOfPledgedVehicles);
                }
            }
            return getOffers;
        }
        private List<Offer> FindOptimalWins(List<Offer> offersToCheck, int numberOfPledgedVehicles)
        {
            List<Offer> offersWithConflict = new List<Offer>();
            List<Offer> offersToChooseFrom = offersToCheck.OrderByDescending(x => x.DifferenceToNextOffer).ThenBy(x => x.ContractorPriority).ToList();
            int eligibleOffers = 0;
            foreach (Offer offer in offersToChooseFrom)
            {
                if (offer.DifferenceToNextOffer >= offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer)
                {
                    offer.IsEligible = true;
                    eligibleOffers++;
                }
                else
                {
                    offer.IsEligible = false;
                }
            }

            if (eligibleOffers <= numberOfPledgedVehicles) return offersWithConflict;
            {
                if (offersToChooseFrom[numberOfPledgedVehicles - 1].ContractorPriority != offersToChooseFrom[numberOfPledgedVehicles].ContractorPriority)
                {
                    for (int i = numberOfPledgedVehicles; i < offersToCheck.Count; i++)
                    {
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if (offersToChooseFrom[i].DifferenceToNextOffer == offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer)
                        {
                            offersToChooseFrom[i].IsEligible = false;
                        }
                    }
                }
                else
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    offersWithConflict.AddRange(offersToChooseFrom.Where(offer => offer.DifferenceToNextOffer == offersToChooseFrom[numberOfPledgedVehicles - 1].DifferenceToNextOffer && offer.IsEligible));
                    if (offersWithConflict.Count == 1)
                    {
                        offersWithConflict.Clear();
                    }
                }
            }
            return offersWithConflict;
        }
        public void CountWonOffersByType(List<Offer> outPutList)
        {
            foreach (Offer offer in outPutList)
            {
                if (offer.UserId != UserId) continue;
                switch (offer.RequiredVehicleType)
                {
                    case 2:
                        NumberOfWonType2Offers++;
                        break;
                    case 3:
                        NumberOfWonType3Offers++;
                        break;
                    case 5:
                        NumberOfWonType5Offers++;
                        break;
                    case 6:
                        NumberOfWonType6Offers++;
                        break;
                    case 7:
                        NumberOfWonType7Offers++;
                        break;
                }
            }
        }
    }
}