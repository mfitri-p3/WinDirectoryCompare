using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinDirectoryCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CoreLibrary CoreLib { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetupBinding();
        }

        #region Setup

        public void SetupBinding()
        {
            CoreLib = new CoreLibrary();
            //Binding sourcePathBinding = new Binding("SourcePath");
            //PathInputSource.SetBinding(TextBox.TextProperty, sourcePathBinding);
            //Binding destinationPathBinding = new Binding("DestinationPath");
            //PathInputDestination.SetBinding(TextBox.TextProperty, destinationPathBinding);
            //Binding sourceFileList = new Binding("SourceFiles");
            //Binding destinationFileList = new Binding("DestinationFiles");
            //SourceListView.SetBinding(ItemsControl.ItemsSourceProperty, sourceFileList);
            //DestinationListView.SetBinding(ItemsControl.ItemsSourceProperty, destinationFileList);
            SourceListView.ItemsSource = CoreLib.SourceFiles;
            DestinationListView.ItemsSource = CoreLib.DestinationFiles;
        }

        #endregion

        private void ConfirmSourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PathInputSource.Text))
            {
                CoreLib.SourcePath = PathInputSource.Text.Trim();
            }
            else
            {
                MessageBox.Show("Provided Source Path is empty!", "Source Path", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ConfirmDestinationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PathInputDestination.Text))
            {
                CoreLib.DestinationPath = PathInputDestination.Text.Trim();
            }
            else
            {
                MessageBox.Show("Provided Destination Path is empty!", "Destination Path", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CopyFilesButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errMsgFromCore = new StringBuilder();
            CoreLib.TransferMissingToDestination(out errMsgFromCore);
            if (errMsgFromCore.Length > 0)
            {
                MessageBox.Show(errMsgFromCore.ToString(), "Error Message when Copying Files", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowDifferenceButton_Click(object sender, RoutedEventArgs e)
        {
            CoreLib.FindDifference();
        }

        private void ClearFilesButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
