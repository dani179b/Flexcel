using Domain;
using System;
using System.ComponentModel;
using System.Windows;

namespace View
{
    public partial class MainWindow
    {
        public readonly MainWindowViewModel MainWindowViewModel = new MainWindowViewModel();
        public ContractorOld Contractor = new ContractorOld();

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void BtnMasterDataFilePathSelect_Click(object sender, RoutedEventArgs e)
        {
            txtBoxFilePathMasterData.Text = MainWindowViewModel.ChooseCsvFile();
        }
        private void BtnRouteNumberFilePathSelect_Click(object sender, RoutedEventArgs e)
        {
            txtBoxFilePathRouteNumberOffer.Text = MainWindowViewModel.ChooseCsvFile();
        }
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBoxFilePathMasterData.Text.Equals("") || txtBoxFilePathRouteNumberOffer.Text.Equals(""))
                {
                    MessageBox.Show("Vælg venligst begge filer inden import startes");
                }
                else if ((txtBoxFilePathMasterData.Text.Equals("") && txtBoxFilePathRouteNumberOffer.Text.Equals("")))
                {
                    MessageBox.Show("Vælg venligst filerne inden import startes");
                }
                else
                {
                    MainWindowViewModel.ImportCSV(txtBoxFilePathMasterData.Text, txtBoxFilePathRouteNumberOffer.Text);
                    MessageBox.Show("Filerne er nu importeret");
                    MainWindowViewModel.ImportDone = true;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

        }
        private void BtnStartSelection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindowViewModel.InitializeSelection();
                ListContainer listContainer = ListContainer.GetInstance();
                //List<Offer> outputListByUserId = listContainer.OutputList.OrderBy(x => x.UserId).ToList();
                //listView.ItemsSource = outputListByUserId;
                foreach (Offer unused in listContainer.OutputList)
                {
                    Contractor.CountWonOffersByType(listContainer.OutputList);
                }
                MessageBox.Show("Udvælgelsen er nu færdig");
            }
            catch (Exception x)
            {
                PromptWindow promptWindow = new PromptWindow(x.Message);
                promptWindow.Show();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Environment.Exit(1);
        }

        private void btnSavePublic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindowViewModel.SaveCsvPublishFile();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void btnSaveCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindowViewModel.SaveCsvCallFile();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
    }
}
