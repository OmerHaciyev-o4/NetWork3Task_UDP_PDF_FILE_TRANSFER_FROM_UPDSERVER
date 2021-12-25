using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;
using mdt=MaterialDesignThemes.Wpf;

namespace UPDServer.MVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Private Variable

        private TcpListener _listener;

        #endregion

        #region Auto Property

        //public ObservableCollection<ListBoxItem> AllInfo { get; set; }

        private ObservableCollection<ListBoxItem> allInfo;
        public ObservableCollection<ListBoxItem> AllInfo
        {
            get { return allInfo; }
            set { allInfo = value; OnPropertyChanged(); }
        }


        #endregion

        #region References

        public MainWindow MainWindow { get; set; }

        #endregion


        public MainViewModel()
        {
            AllInfo = new ObservableCollection<ListBoxItem>();
            Thread getData = new Thread(() =>
            {
                Receive();
            });
            getData.Start();
        }

        private void Receive()
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                var ipAddress = IPAddress.Any;
                var port = 11804;
                var endPoint = new IPEndPoint(ipAddress, port);

                _listener = new TcpListener(endPoint);
                _listener.Start();

                while (true)
                {
                    var client = _listener.AcceptTcpClient();

                    Task.Run(() =>
                    {
                        while (true)
                        {
                            var stream = client.GetStream();

                            BinaryReader _br = new BinaryReader(stream);

                            try
                            {
                                client.ReceiveBufferSize = 1000050000;
                                var bytes = _br.ReadBytes(client.ReceiveBufferSize);

                                var infos = Encoding.ASCII.GetString(bytes).Split('\t');

                                if (infos != null && infos.Length > 0 && infos[0] != "")
                                {
                                    var path = string.Empty;
                                    var clientName = string.Empty;

                                    infos[0] = infos[0].Remove(infos[0].Length - 1);
                                    var infoStrArray = infos[0].Split('\n');
                                    var infoByteArray = infoStrArray.Select(byte.Parse).ToArray();
                                    var newInfoByteArray = Encoding.ASCII.GetString(infoByteArray).Split('\n');
                                    path = newInfoByteArray[0];
                                    clientName = newInfoByteArray[1];

                                    infos[2] = infos[2].Remove(infos[2].Length - 1);
                                    var fileStrArray = infos[2].Split('\n');
                                    var fielByteArray = fileStrArray.Select(byte.Parse).ToArray();

                                    path = CreateAndSaveFile(fielByteArray, path);

                                    ListBoxItem item = null;
                                    Grid grid = null;
                                    Label clName = null;
                                    Button run = null;

                                    App.Current.Dispatcher.Invoke(() =>
                                    {
                                        mdt.PackIcon icon = new mdt.PackIcon();
                                        icon.Kind = mdt.PackIconKind.Play;
                                        icon.Width = 20;
                                        icon.Height = 20;

                                        run = new Button();
                                        run.Uid = path;
                                        run.Background = new SolidColorBrush(Colors.Green);
                                        run.Click += RunMethod;
                                        run.Content = icon;

                                        clName = new Label();
                                        clName.Content = "Client name: " + clientName;
                                        clName.FontSize = 15;
                                        clName.Foreground = new SolidColorBrush(Colors.White);

                                        StackPanel panel = new StackPanel() { Width = 275 };
                                        panel.Children.Add(clName);
                                        panel.Children.Add(run);

                                        item = new ListBoxItem();
                                        item.Content = panel;

                                        AllInfo.Add(item);
                                    });
                                }
                            }
                            catch (Exception ){}
                        }
                    });
                }

            }
        }

        private string CreateAndSaveFile(byte[] bytes, string path)
        {
            var currentPath = Directory.GetCurrentDirectory();

            var tempData = currentPath.Split('\\');

            currentPath = string.Empty;

            for (int i = 0; i < tempData.Length - 2; i++)
                currentPath += tempData[i] + "\\";

            tempData = path.Split('\\');
            currentPath += "MVVM\\Resource\\" + tempData[tempData.Length - 1];

            File.WriteAllBytes(currentPath, bytes);

            return currentPath;
        }

        private void RunMethod(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;

            var path = button.Uid;

            Process.Start(path);
        }
    }
}