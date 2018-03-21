using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess
{
    public class CSVExportToCallList
    {
        private readonly Encoding _encoding;
        private readonly string _filePath;
        private readonly List<Offer> _winningOfferList;
        public CSVExportToCallList(string filePath)
        {
            _filePath = filePath;
            ListContainer listContainer = ListContainer.GetInstance();
            _winningOfferList = listContainer.OutputList;
            _encoding = Encoding.GetEncoding("iso-8859-1");
        }

        public void CreateFile()
        {
            try
            {
                // Delete the file if it exists.
                if (File.Exists(_filePath))
                {
                    // Note that no lock is put on the
                    // file and the possibility exists
                    // that another process could do
                    // something with it between
                    // the calls to Exists and Delete.
                    File.Delete(_filePath);
                }
                // Create the file.
                using (StreamWriter streamWriter = new StreamWriter(_filePath, true, _encoding))
                {
                    streamWriter.WriteLine($"Nummer;Virksomhedsnavn;Navn;Vedståede v. 2;Vedståede v. 3;Vedståede v. 5" + ";Vedståede v. 6;Vedståede v. 7;Vundne v. 2;Vundne v. 3;Vundne v. 5;Vundne v. 6;Vundne v. 7");
                    foreach (Offer offer in _winningOfferList)
                    {
                        streamWriter.WriteLine($"{offer.OfferReferenceNumber};{offer.Contractor.CompanyName};{offer.Contractor.ManagerName};{offer.Contractor.NumberOfType2PledgedVehicles};{offer.Contractor.NumberOfType3PledgedVehicles};{offer.Contractor.NumberOfType5PledgedVehicles};{offer.Contractor.NumberOfType6PledgedVehicles};{offer.Contractor.NumberOfType7PledgedVehicles};{offer.Contractor.NumberOfWonType2Offers};{offer.Contractor.NumberOfWonType3Offers};{offer.Contractor.NumberOfWonType5Offers};{offer.Contractor.NumberOfWonType6Offers};{offer.Contractor.NumberOfWonType7Offers};");
                    }
                    streamWriter.Close();
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(_filePath))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Filen blev ikke gemt");
            }
        }


    }
}