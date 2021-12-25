using System.Windows;
using UPDServer.MVVM.ViewModels;

namespace UPDServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainViewModel() { MainWindow = this };
            this.DataContext = vm;
        }
    }
}