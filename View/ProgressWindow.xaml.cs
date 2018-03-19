using System;
using System.Windows;
using System.Threading;
using Logic;
using System.Windows.Threading;

namespace View
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        public void UpdateLabelContent()
        {
            this.lblStatusText.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {

                this.lblStatusText.Content = SelectionController.progressStatusText.ToString();

                RecursionUpdateLabelContent();

            }));
        }
        public void RecursionUpdateLabelContent()
        {
            if (SelectionController.progressStatusText == "Udvælgelse Færdig")
            {
                pbWorking.IsIndeterminate = false;
                pbWorking.Value = 100;
                MessageBox.Show("Udvælgelse færdig");
                Thread.CurrentThread.Abort();
            }
            else
            {
                RecursionUpdateLabelContent();
            }
        }
    }
}