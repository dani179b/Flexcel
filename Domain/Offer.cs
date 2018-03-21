namespace Domain
{
    public class Offer
    {
        public string OfferReferenceNumber { get; set; }
        public float OperationPrice { get; set; }
        public bool IsEligible { get; set; }
        public int RequiredVehicleType { get; set; }
        public int RouteId { get; set; }
        public string UserId { get; set; }
        public float DifferenceToNextOffer { get; set; }
        public string CreateRouteNumberPriority { get; set; }
        public string CreateContractorPriority { get; set; }
        public int RouteNumberPriority { get; set; }
        public int ContractorPriority { get; set; }
        public ContractorOld Contractor { get; set; }

        public Offer() { }
        public Offer(string referenceNumber, float operationPrice, int routeId, string userId, int routeNumberPriority, int contractorPriority, ContractorOld contractor, int requiredVehicleType = 0)
        {
            OfferReferenceNumber = referenceNumber;
            OperationPrice = operationPrice;
            RouteId = routeId;
            UserId = userId;
            RouteNumberPriority = routeNumberPriority;
            ContractorPriority = contractorPriority;
            Contractor = contractor;
            RequiredVehicleType = requiredVehicleType;
            IsEligible = true;
        }
    }
}
