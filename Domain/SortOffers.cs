using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class SortOffers
    {
        public List<Offer> WinningOffers;
        public List<Offer> IneligibleOffers;
        public List<Offer> OffersWithConflict;

        public List<Offer> OfferVehicleType2;
        public List<Offer> OfferVehicleType3;
        public List<Offer> OfferVehicleType5;
        public List<Offer> OfferVehicleType6;
        public List<Offer> OfferVehicleType7;

        public int Type2PledgedVehicles { get; set; }
        public int Type3PledgedVehicles { get; set; }
        public int Type5PledgedVehicles { get; set; }
        public int Type6PledgedVehicles { get; set; }
        public int Type7PledgedVehicles { get; set; }
        public string TryParseValueType2PledgedVehicles { get; set; }
        public string TryParseValueType3PledgedVehicles { get; set; }
        public string TryParseValueType5PledgedVehicles { get; set; }
        public string TryParseValueType6PledgedVehicles { get; set; }
        public string TryParseValueType7PledgedVehicles { get; set; }
        public int WonType2Offers { get; private set; }
        public int WonType3Offers { get; private set; }
        public int WonType5Offers { get; private set; }
        public int WonType6Offers { get; private set; }
        public int WonType7Offers { get; private set; }

        public SortOffers()
        {
            WinningOffers = new List<Offer>();
            IneligibleOffers = new List<Offer>();
        }

        public SortOffers(int type2PledgedVehicles, int type3PledgedVehicles, int type5PledgedVehicles,
            int type6PledgedVehicles, int type7PledgedVehicles)
        {
            Type2PledgedVehicles = type2PledgedVehicles;
            Type3PledgedVehicles = type3PledgedVehicles;
            Type5PledgedVehicles = type5PledgedVehicles;
            Type6PledgedVehicles = type6PledgedVehicles;
            Type7PledgedVehicles = type7PledgedVehicles;
        }

        public void AddWonOffer(Offer offer)
        {
            bool alreadyOnTheList = WinningOffers.Any(item => item.OfferReferenceNumber == offer.OfferReferenceNumber);
            if (!alreadyOnTheList)
            {
                WinningOffers.Add(offer);
                offer.IsEligible = true;

                switch (offer.RequiredVehicleType)
                {
                    case 2:
                        OfferVehicleType2.Add(offer);
                        break;
                    case 3:
                        OfferVehicleType3.Add(offer);
                        break;
                    case 5:
                        OfferVehicleType5.Add(offer);
                        break;
                    case 6:
                        OfferVehicleType6.Add(offer);
                        break;
                    case 7:
                        OfferVehicleType7.Add(offer);
                        break;
                }
            }
        }

        public void CountWonOffersByType(List<Offer> offers)
        {
            Contractor c = new Contractor();
            foreach (Offer offer in offers)
            {
                if (offer.UserId == c.UserId)
                {
                    switch (offer.RequiredVehicleType)
                    {
                        case 2:
                            WonType2Offers++;
                            break;
                        case 3:
                            WonType3Offers++;
                            break;
                        case 5:
                            WonType5Offers++;
                            break;
                        case 6:
                            WonType6Offers++;
                            break;
                        case 7:
                            WonType7Offers++;
                            break;
                    }
                }
            }
        }
    }
}