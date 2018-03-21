using DataAccess;
using Domain;
using System.Collections.Generic;

namespace Logic
{
    public class IoController
    {
        private List<RouteNumber> _routeNumberList;
        private List<ContractorOld> _contractorList;

        public IoController()
        {
            _routeNumberList = new List<RouteNumber>();
        }

        public void InitializeExportToPublishList(string filePath)
        {
            CSVExportToPublishList exportToPublishList = new CSVExportToPublishList(filePath);
            exportToPublishList.CreateFile();
        }
        public void InitializeExportToCallingList(string filePath)
        {
            CSVExportToCallList exportCallList = new CSVExportToCallList(filePath);
            exportCallList.CreateFile();
        }
        public void InitializeImport(string masterDataFilepath, string routeNumberFilepath)
        {
            CsvImport csvImport = new CsvImport();
            csvImport.ImportContractors(masterDataFilepath);
            csvImport.ImportRouteNumbers();
            csvImport.ImportOffers(routeNumberFilepath);
            _contractorList = csvImport.Contractors;
            _routeNumberList = csvImport.RouteNumbers;
            ListContainer listContainer = ListContainer.GetInstance();
            listContainer.GetLists(_routeNumberList, _contractorList);
        }
    }
}
