using Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DataAccess
{
    public class CsvImport
    {
        private readonly Encoding _encoding;
        public List<ContractorOld> Contractors;
        public List<RouteNumber> RouteNumbers;
        public List<Offer> Offers;
        public CsvImport()
        {
            Contractors = new List<ContractorOld>();
            RouteNumbers = new List<RouteNumber>();
            Offers = new List<Offer>();
            _encoding = Encoding.GetEncoding("iso-8859-1");
        }
        public int TryParseToIntElseZero(string toParse)
        {
            toParse = toParse.Replace(" ", "");
            // ReSharper disable once UnusedVariable
            bool tryParse = int.TryParse(toParse, out int number);
            return number;
        }
        public float TryParseToFloatElseZero(string toParse)
        {
            string currentCultureName = Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo cultureInformation = new CultureInfo(currentCultureName);
            if (cultureInformation.NumberFormat.NumberDecimalSeparator != ",")
            // Forcing use of decimal separator for numerical values
            {
                cultureInformation.NumberFormat.NumberDecimalSeparator = ",";
                Thread.CurrentThread.CurrentCulture = cultureInformation;
            }
            toParse = toParse.Replace(" ", "");
            // ReSharper disable once UnusedVariable
            bool tryParse = float.TryParse(toParse.Replace('.', ','), out float number);
            return number;
        }
        public void ImportOffers(string filepath)
        {
            try
            {
                IEnumerable<Offer> data = File.ReadAllLines(filepath, _encoding)
               .Skip(1)
               .Select(x => x.Split(';'))
               .Select(x => new Offer
               {
                   OfferReferenceNumber = x[0],
                   RouteId = TryParseToIntElseZero(x[1]),
                   OperationPrice = TryParseToFloatElseZero((x[2])),
                   UserId = x[5],
                   CreateRouteNumberPriority = x[6],
                   CreateContractorPriority = x[7],
               });
                foreach (Offer o in data)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (o.UserId == "" && o.OperationPrice == 0) continue;
                    o.RouteNumberPriority = TryParseToIntElseZero(o.CreateRouteNumberPriority);
                    o.ContractorPriority = TryParseToIntElseZero(o.CreateContractorPriority);
                    ContractorOld contractor = Contractors.Find(x => x.UserId == o.UserId);
                    try
                    {
                        o.RequiredVehicleType = RouteNumbers.Find(r => r.RouteId == o.RouteId).RequiredVehicleType;
                        Offer newOffer = new Offer(o.OfferReferenceNumber, o.OperationPrice, o.RouteId, o.UserId, o.RouteNumberPriority, o.ContractorPriority, contractor, o.RequiredVehicleType);
                        Offers.Add(newOffer);
                    }
                    catch
                    {
                        // Help for debugging purpose only.
                        // ReSharper disable once UnusedVariable
                        string failure = o.RouteId.ToString();
                    }
                }
                foreach (RouteNumber routeNumber in RouteNumbers)
                {
                    foreach (Offer offer in Offers)
                    {
                        if (offer.RouteId == routeNumber.RouteId)
                        {
                            routeNumber.offers.Add(offer);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (FormatException)
            {
                throw new FormatException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (Exception)
            {
                throw new Exception("Fejl, filerne blev ikke importeret");
            }
        }
        public void ImportRouteNumbers()
        {
            try
            {
                string filepath = Environment.ExpandEnvironmentVariables("RouteNumbers.csv");
                IEnumerable<RouteNumber> data = File.ReadAllLines(filepath, _encoding)
                .Skip(1)
                .Select(x => x.Split(';'))
                .Select(x => new RouteNumber
                {
                    RouteId = TryParseToIntElseZero(x[0]),
                    RequiredVehicleType = TryParseToIntElseZero(x[1]),
                });
                foreach (var r in data)
                {
                    bool doesAlreadyContain = RouteNumbers.Any(obj => obj.RouteId == r.RouteId);

                    if (!doesAlreadyContain && r.RouteId != 0 && r.RequiredVehicleType != 0)
                    {
                        RouteNumbers.Add(r);
                    }
                }

            }


            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (FormatException)
            {
                throw new FormatException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (Exception)
            {
                throw new Exception("Fejl, filerne blev ikke importeret");
            }
        }
        public void ImportContractors(string filepath)
        {
            try
            {
                IEnumerable<ContractorOld> data = File.ReadAllLines(filepath, _encoding)
                  .Skip(1)
                  .Select(x => x.Split(';'))
                  .Select(x => new ContractorOld
                  {
                      ReferenceNumberBasicInformationPdf = x[0],
                      ManagerName = x[1],
                      CompanyName = x[2],
                      UserId = x[3],
                      TryParseValueType2PledgedVehicles = x[4],
                      TryParseValueType3PledgedVehicles = x[5],
                      TryParseValueType5PledgedVehicles = x[6],
                      TryParseValueType6PledgedVehicles = x[7],
                      TryParseValueType7PledgedVehicles = x[8],

                  });

                foreach (ContractorOld c in data)
                {
                    if (c.UserId == "") continue;
                    {
                        //bool doesAlreadyContain = Contractors.Any(obj => obj.UserID == c.UserID);

                        c.NumberOfType2PledgedVehicles = TryParseToIntElseZero(c.TryParseValueType2PledgedVehicles);
                        c.NumberOfType3PledgedVehicles = TryParseToIntElseZero(c.TryParseValueType3PledgedVehicles);
                        c.NumberOfType5PledgedVehicles = TryParseToIntElseZero(c.TryParseValueType5PledgedVehicles);
                        c.NumberOfType6PledgedVehicles = TryParseToIntElseZero(c.TryParseValueType6PledgedVehicles);
                        c.NumberOfType7PledgedVehicles = TryParseToIntElseZero(c.TryParseValueType7PledgedVehicles);

                        ContractorOld newContractor = new ContractorOld(
                            c.ReferenceNumberBasicInformationPdf,
                            c.UserId, c.CompanyName, c.ManagerName,
                            c.NumberOfType2PledgedVehicles, c.NumberOfType3PledgedVehicles,
                            c.NumberOfType5PledgedVehicles, c.NumberOfType6PledgedVehicles,
                            c.NumberOfType7PledgedVehicles);
                        Contractors.Add(newContractor);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (FormatException)
            {
                throw new FormatException("Fejl, er du sikker på du har valgt den rigtige fil?");
            }
            catch (Exception)
            {
                throw new Exception("Fejl, filerne blev ikke importeret");
            }
        }
    }
}
