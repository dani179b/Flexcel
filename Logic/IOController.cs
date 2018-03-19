using DataAccess;
using Domain;
using System.Collections.Generic;

namespace Logic
{
    public class IOController
    {
        List<RouteNumber> routeNumberList;
        List<ContractorOLD> contractorList;

        public IOController()
        {
            routeNumberList = new List<RouteNumber>();
        }

        public void InitializeExportToPublishList(string filePath)
        {
            CSVExportToPublishList ExportToPublishList = new CSVExportToPublishList(filePath);
            ExportToPublishList.CreateFile();
        }
        public void InitializeExportToCallingList(string filePath)
        {
            CSVExportToCallList ExportCallList = new CSVExportToCallList(filePath);
            ExportCallList.CreateFile();
        }
        public void InitializeImport(string masterDataFilepath, string routeNumberFilepath)
        {
            CSVImport csvImport = new CSVImport();
            csvImport.ImportContractors(masterDataFilepath);
            csvImport.ImportRouteNumbers();
            csvImport.ImportOffers(routeNumberFilepath);
            contractorList = csvImport.Contractors;
            routeNumberList = csvImport.RouteNumbers;
            ListContainer listContainer = ListContainer.GetInstance();
            listContainer.GetLists(routeNumberList, contractorList);
        }
    }
}
