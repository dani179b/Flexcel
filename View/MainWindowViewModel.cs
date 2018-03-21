using Logic;
using Microsoft.Win32;
using System.Windows;

namespace View
{
    public class MainWindowViewModel
    {
        private readonly IoController _iOController;
        private readonly SelectionController _selectionController;
        private OpenFileDialog _openFileDialog;

        public bool ImportDone { get; set; }
        public bool SelectionDone { get; set; }

        public MainWindowViewModel()
        {
            _iOController = new IoController();
            _selectionController = new SelectionController();
            ImportDone = false;
        }

        public void ImportCSV(string masterDataFilepath, string routeNumberFilepath)
        {
            _iOController.InitializeImport(masterDataFilepath, routeNumberFilepath);
        }

        public string ChooseCsvFile()
        {
            string filename = "Ingen fil er valgt";
            _openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "CVS filer (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (_openFileDialog.ShowDialog() != true) return filename;
            filename = _openFileDialog.FileName;
            return filename;
        }

        public void SaveCsvCallFile()
        {
            if (SelectionDone == true)
            {
                SaveFileDialog saveDlg = new SaveFileDialog
                {
                    Filter = "CSV filer (*.csv)|*.csv|All files (*.*)|*.*",
                    InitialDirectory = @"C:\%USERNAME%\"
                };

                saveDlg.ShowDialog();

                string path = saveDlg.FileName;
                _iOController.InitializeExportToCallingList(path);
                MessageBox.Show("Filen er gemt.");
            }
            else
            {
                MessageBox.Show("Du har ikke udvalgt vinderne endnu.. Kør Udvælgelse først!");
            }
        }
        public void SaveCsvPublishFile()
        {
            if (SelectionDone)
            {
                SaveFileDialog saveDlg = new SaveFileDialog
                {
                    Filter = "CSV filer (*.csv)|*.csv|All files (*.*)|*.*",
                    InitialDirectory = @"C:\%USERNAME%\"
                };

                saveDlg.ShowDialog();

                string path = saveDlg.FileName;

                _iOController.InitializeExportToPublishList(path);
                MessageBox.Show("Filen er gemt.");
            }
            else
            {
                MessageBox.Show("Du har ikke udvalgt vinderne endnu.. Kør Udvælgelse først!");
            }
        }
        public void InitializeSelection()
        {
            if (ImportDone)
            {
                SelectionDone = true;
            }
            else
            {
                MessageBox.Show("Du skal importere filerne først.");
            }
            _selectionController.SelectWinners();
        }
    }
}
