using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess
{
    public class CSVExportToPublishList
    {
        Encoding encoding;
        string FilePath;
        ListContainer listContainer;
        List<Offer> winningOfferList;

        public CSVExportToPublishList(string filePath)
        {
            FilePath = filePath;
            listContainer = ListContainer.GetInstance();
            winningOfferList = listContainer.OutputList;
            encoding = Encoding.GetEncoding("iso-8859-1");
        }

        public void CreateFile()
        {
            try
            {
                // Delete the file if it exists.
                if (File.Exists(FilePath))
                {
                    // Note that no lock is put on the
                    // file and the possibility exists
                    // that another process could do
                    // something with it between
                    // the calls to Exists and Delete.
                    File.Delete(FilePath);
                }
                // Create the file.
                using (StreamWriter streamWriter = new StreamWriter(@FilePath, true, encoding))
                {
                    streamWriter.WriteLine("Garantivognsnummer" + ";" + "Virksomhedsnavn" + ";" + "Pris" + ";");
                    foreach (Offer offer in winningOfferList)
                    {
                        streamWriter.WriteLine(offer.RouteId + ";" + offer.Contractor.CompanyName + ";" + offer.OperationPrice + ";");
                    }
                    streamWriter.Close();
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(FilePath))
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
