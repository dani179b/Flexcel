using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess
{
    public class CSVExportToPublishList
    {
        private readonly Encoding _encoding;
        private readonly string _filePath;
        private readonly List<Offer> _winningOfferList;

        public CSVExportToPublishList(string filePath)
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
                    streamWriter.WriteLine("Garantivognsnummer" + ";" + "Virksomhedsnavn" + ";" + "Pris" + ";");
                    foreach (Offer offer in _winningOfferList)
                    {
                        streamWriter.WriteLine(offer.RouteId + ";" + offer.Contractor.CompanyName + ";" + offer.OperationPrice + ";");
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
