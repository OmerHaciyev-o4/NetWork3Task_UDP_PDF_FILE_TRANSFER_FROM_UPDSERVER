using System.Windows;
using UDPClient.MVVM.ViewModels;

namespace UDPClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new ViewModel() { MainWindow = this };
            this.DataContext = vm;
        }
    }
}
