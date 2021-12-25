using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UDPClient.MVVM.Commands;

namespace UDPClient.MVVM.ViewModels
{
    public class ViewModel : BaseViewModel
    {
        #region Private Varibale

        private BinaryWriter _bw;
        private string _path;
        private bool _connectionState;
        private string _clientName;

        #endregion

        #region Commands

        public ICommand ConnectionCommand { get; set; }
        public ICommand DisconnectionCommand { get; set; }

        #endregion

        #region FullProperty

        private string state;
        public string State
        {
            get { return state; }
            set { state = value; OnPropertyChanged(); }
        }


        #endregion

        #region References

        public MainWindow MainWindow { get; set; }

        #endregion

        #region UDPInfos

        private Socket socket;
        private IPAddress ipAddress;
        private int port;
        private IPEndPoint endPoint;
        private TcpClient client;

        #endregion

        public ViewModel()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ipAddress = IPAddress.Parse("192.168.1.100");
            port = 11804;
            endPoint = new IPEndPoint(ipAddress, port);
            State = "Server disconnect";

            ConnectionCommand = new RelayCommand((o) =>
            {
                Task.Run(() =>
                {
                    string text = string.Empty;
                    App.Current.Dispatcher.Invoke(() => { text = MainWindow.NameBox.Text; });

                    if (text == string.Empty)
                    {
                        MessageBox.Show("Please enter name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    try
                    {
                        client = new TcpClient();
                        client.Client.Connect(endPoint);

                        if (client.Client.Connected)
                        {
                            _connectionState = true;
                            MessageBox.Show("Successfully connected to the server.", "Information",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                            App.Current.Dispatcher.Invoke(() =>
                            {
                                MainWindow.DragDropListBox.Drop += DragDropListBoxOnDrop;
                                MainWindow.DragDropListBox.AllowDrop = true;
                                State = "Server Connection";
                                _clientName = MainWindow.NameBox.Text;
                            });
                        }
                        else
                            MessageBox.Show("Please check 'EndPoint'", "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Server not working", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
            , (pre) =>
            {
                if (!_connectionState)
                    return true;
                else return false;
            });

            DisconnectionCommand = new RelayCommand((o) =>
            {
                Task.Run(() =>
                {
                    client.Client.Close();
                    _connectionState = false;

                    MessageBox.Show("The server was successfully disconnected.", "information", MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        MainWindow.DragDropListBox.Drop -= DragDropListBoxOnDrop;
                        MainWindow.DragDropListBox.AllowDrop = false;
                    });

                    State = "Server disconnect";
                });
            }
            , (pred) =>
            {
                if (_connectionState)
                    return true;
                else return false;
            });

            Thread setReferenceDatas = new Thread(() =>
            {
                setDataMethod();
            });
            setReferenceDatas.Start();
        }

        private void setDataMethod()
        {
            MainWindow.NameBox.TextChanged += NameBoxOnTextChanged;
        }

        private void DragDropListBoxOnDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            _path = fileList[0];

            fileList = _path.Split('\\');

            State = fileList[fileList.Length - 1] + "   Sending . . .";

            App.Current.Dispatcher.Invoke(() =>
            {
                MainWindow.DragDropListBox.Drop -= DragDropListBoxOnDrop;
            });

            Thread thread = new Thread(() =>
            {
                string info = _path + "\n" + _clientName;

                byte[] fileBytes = File.ReadAllBytes(_path);
                var fileStrArray = fileBytes.Select(byteValue => byteValue.ToString()).ToArray();

                fileBytes = Encoding.ASCII.GetBytes(info);
                var infoStrArray = fileBytes.Select(byteValue => byteValue.ToString()).ToArray();

                var infoStr = string.Empty;
                var fileStr = string.Empty;

                foreach (var s in infoStrArray)
                    infoStr += s + '\n';

                foreach (var s in fileStrArray)
                    fileStr += s + '\n';

                var allInfo = infoStr + "\t\t" + fileStr;

                var allInfoBytes = Encoding.ASCII.GetBytes(allInfo);

                var stream = client.GetStream();
                _bw = new BinaryWriter(stream);
                _bw.Write(allInfoBytes);

                client.Dispose();
                client = new TcpClient();
                client.Client.Connect(endPoint);

                App.Current.Dispatcher.Invoke(() => { State = "The case was sent successfully"; });

                Thread.Sleep(4000);

                App.Current.Dispatcher.Invoke(() =>
                {
                    State = "Listening.";
                    MainWindow.DragDropListBox.Drop += DragDropListBoxOnDrop;
                });
                
            });
            thread.Start();
        }

        private void NameBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = string.Empty;
            App.Current.Dispatcher.Invoke(() => { text = MainWindow.NameBox.Text; });

            if (!Regex.IsMatch(text, "^[A-Za-z]"))
            {
                if (text.Length == 0)
                    text = string.Empty;
                else 
                    text = text.Remove(text.Length - 1);

                App.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow.NameBox.Text = text;
                    if (text.Length > 0)
                        MainWindow.NameBox.SelectionStart = text.Length - 1;
                    MainWindow.NameBox.SelectionLength = 0;
                });
            }
        }
    }
}