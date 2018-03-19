using Logic;
using Microsoft.Win32;
using System.Windows;

namespace View
{
    public class MainWindowViewModel
    {
        IOController iOController;
        SelectionController selectionController;
        OpenFileDialog openFileDialog;

        public bool ImportDone { get; set; }
        public bool SelectionDone { get; set; }

        public MainWindowViewModel()
        {
            iOController = new IOController();
            selectionController = new SelectionController();
            ImportDone = false;
        }

        public void ImportCSV(string masterDataFilepath, string routeNumberFilepath)
        {
            iOController.InitializeImport(masterDataFilepath, routeNumberFilepath);
        }

        public string ChooseCSVFile()
        {
            string filename = "Ingen fil er valgt";
            openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "CVS filer (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                return filename;
            }
            return filename;
        }

        public void SaveCSVCallFile()
        {
            if (SelectionDone == true)
            {
                SaveFileDialog saveDlg = new SaveFileDialog();

                saveDlg.Filter = "CSV filer (*.csv)|*.csv|All files (*.*)|*.*";
                saveDlg.InitialDirectory = @"C:\%USERNAME%\";
                saveDlg.ShowDialog();

                string path = saveDlg.FileName;
                iOController.InitializeExportToCallingList(path);
                MessageBox.Show("Filen er gemt.");
            }
            else
            {
                MessageBox.Show("Du har ikke udvalgt vinderne endnu.. Kør Udvælgelse først!");
            }
        }
        public void SaveCSVPublishFile()
        {
            if (SelectionDone == true)
            {
                SaveFileDialog saveDlg = new SaveFileDialog();

                saveDlg.Filter = "CSV filer (*.csv)|*.csv|All files (*.*)|*.*";
                saveDlg.InitialDirectory = @"C:\%USERNAME%\";
                saveDlg.ShowDialog();

                string path = saveDlg.FileName;

                iOController.InitializeExportToPublishList(path);
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
            selectionController.SelectWinners();
        }
    }
}
