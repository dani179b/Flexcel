using Domain;
using System;
using System.ComponentModel;
using System.Windows;

namespace View
{
    /// <summary>
    /// Interaction logic for PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        ListContainer listContainer = ListContainer.GetInstance();
        public PromptWindow(string message)
        {
            InitializeComponent();
            listView.ItemsSource = listContainer.ConflictList;
            tbConflictMessage.Text = message;
        }

        private void btnPromptOkay_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Environment.Exit(1);
        }
    }
}
