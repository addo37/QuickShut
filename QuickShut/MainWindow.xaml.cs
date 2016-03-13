using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Hardcodet.Wpf.TaskbarNotification;

namespace QuickShut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Process process;
        private LowLevelKeyboardListener _listener;
        List<Key> _keys;
        private TaskbarIcon tb;
        bool enabled;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _keys = new List<Key>();
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OKP;
            _listener.HookKeyboard();
            process = new Process();
            tb = (TaskbarIcon)FindResource("MyNotifyIcon");

            enabled = true;
        }

        void _listener_OKP(object sender, KeyPressedArgs e)
        {
            _keys.Add(e.KeyPressed);
            if (_keys.Count > 3)
            {
                _keys.Clear();
                _keys.Add(e.KeyPressed);
            }
            if (_keys.Count == 3)
            {
                if (_keys.Exists(ByName(Key.RightCtrl)) && _keys.Exists(ByName(Key.RightAlt)))
                {
                    _keys.Remove(Key.RightCtrl);
                    _keys.Remove(Key.RightAlt);
                    switch(_keys[0])
                    {
                        case Key.D0:
                            clearSD();
                            break;
                        case Key.D1:
                            runSD(1);
                            break;
                        case Key.D2:
                            runSD(2);
                            break;
                        case Key.D3:
                            runSD(3);
                            break;
                        case Key.D4:
                            runSD(4);
                            break;
                        case Key.D5:
                            runSD(5);
                            break;
                        case Key.D6:
                            runSD(6);
                            break;
                        case Key.D7:
                            runSD(7);
                            break;
                        case Key.D8:
                            runSD(8);
                            break;
                        case Key.D9:
                            runSD(9);
                            break;
                    }
                    if (_keys[0] != Key.RightAlt && _keys[0] != Key.RightCtrl)
                        _keys.Clear();
                }
            }
        }

        private void runSD(int hours)
        {
            process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown.exe -s -t " +  3600*hours;
            process.StartInfo = startInfo;
            process.Start();
            process.Close();
           // MenuItem temp = (MenuItem)tb.FindResource("status");
           // temp.Header = "Status: " + hours;
        }

        private void clearSD()
        {
            process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C shutdown.exe/a";
            process.StartInfo = startInfo;
            process.Start();
            process.Close();
           // MenuItem temp = (MenuItem)tb.FindName("status");
           // temp.Header = "Status: Cleared";

        }
        public Predicate<Key> ByName (Key e)
        {
            return delegate (Key key)
            {
                return key == e;
            };
        }

        private void Disable_Clicked(object sender, RoutedEventArgs e)
        {
            if (enabled)
            {
                ((MenuItem)sender).Header = "Enable"; 
                _listener.UnHookKeyboard();
                enabled = false;
            }
            else
            {
                ((MenuItem)sender).Header = "Disable";
                _listener.HookKeyboard();
                enabled = true;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
