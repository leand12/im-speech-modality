using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using Newtonsoft.Json;
using System.Diagnostics;
using AudioSwitcher.AudioApi.CoreAudio;

using System.Windows.Interop;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;




namespace AppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MmiCommunication mmiC;

        //  new 16 april 2020
        private MmiCommunication mmiSender;
        private LifeCycleEvents lce;
        private MmiCommunication mmic;

        public MainWindow()
        {
            InitializeComponent();


            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();

            // NEW 16 april 2020
            //init LifeCycleEvents..
            lce = new LifeCycleEvents("APP", "TTS", "User1", "na", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode
            // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)
            mmic = new MmiCommunication("localhost", 8000, "User1", "GUI");
            

        }

        private String getFile(Int32 handle)
        {
            return "";
        }


        enum DesktopAccess : uint
        {
            DesktopReadobjects = 0x0001,
            DesktopCreatewindow = 0x0002,
            DesktopCreatemenu = 0x0004,
            DesktopHookcontrol = 0x0008,
            DesktopJournalrecord = 0x0010,
            DesktopJournalplayback = 0x0020,
            DesktopEnumerate = 0x0040,
            DesktopWriteobjects = 0x0080,
            DesktopSwitchdesktop = 0x0100,

            GenericAll = DesktopReadobjects | DesktopCreatewindow | DesktopCreatemenu |
                         DesktopHookcontrol | DesktopJournalrecord | DesktopJournalplayback |
                         DesktopEnumerate | DesktopWriteobjects | DesktopSwitchdesktop
        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);

            Shape _s = null;
            switch ((string)json.recognized[0].ToString())
            {
                case "SQUARE": _s = rectangle;
                    break;
                case "CIRCLE": _s = circle;
                    break;
                case "TRIANGLE": _s = triangle;
                    break;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                switch ((string)json.recognized[1].ToString())
                {
                    case "GREEN":
                        //_s.Fill = Brushes.Green;

                        //var psi = new ProcessStartInfo()
                        //{
                        //    FileName = @"C:\Riot Games",
                        //    UseShellExecute = true
                        //};
                        //Process.Start(psi);


                        break;
                    case "BLUE":
                        //_s.Fill = Brushes.Blue;

                        //Process.Start("calc.exe");
                        //Process.Start("www.google.com");
                        //Process.Start("www.facebook.com");
                        break;
                    case "RED":
                        //_s.Fill = Brushes.Red;

                        Process.Start("microsoft.windows.camera:");

                        /*
                        Process[] processlist = Process.GetProcesses();
                        foreach (Process process in processlist)
                        {
                            if (!String.IsNullOrEmpty(process.MainWindowTitle))
                            {
                                Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);
                            }
                        }
                        */

                        MoveWindowTo(WP_DOWN | WP_RIGHT);

                        //CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                        //Debug.WriteLine("Current Volume:" + defaultPlaybackDevice.Volume);
                        //defaultPlaybackDevice.Volume += 10;
                        break;
                }
            });

            //  new 16 april 2020
            mmic.Send(lce.NewContextRequest());

            string json2 = ""; // "{ \"synthesize\": [";
            json2 += (string)json.recognized[0].ToString()+ " ";
            json2 += (string)json.recognized[1].ToString() + " DONE." ;
            //json2 += "] }";
            /*
             foreach (var resultSemantic in e.Result.Semantics)
            {
                json += "\"" + resultSemantic.Value.Value + "\", ";
            }
            json = json.Substring(0, json.Length - 2);
            json += "] }";
            */
            var exNot = lce.ExtensionNotification(0 + "", 0 + "", 1, json2);
            mmic.Send(exNot);


        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;

        private const int WP_UP = 0b0001;
        private const int WP_DOWN = 0b0010;
        private const int WP_LEFT = 0b0100;
        private const int WP_RIGHT = 0b1000;

        private void MoveWindowTo(int position)
        {
            System.Drawing.Rectangle area = Screen.PrimaryScreen.WorkingArea;
            int x = 0;
            int y = 0;
            int height = area.Height;
            int width = area.Width;

            if ((position & WP_UP) > 0)
            {
                height /= 2;
            }
            else if ((position & WP_DOWN) > 0)
            {
                height /= 2;
                y = height;
            }
            if ((position & WP_LEFT) > 0)
            {
                width /= 2;
            }
            else if ((position & WP_RIGHT) > 0)
            {
                width /= 2;
                x = width;
            }

            Process process = Process.GetProcessesByName("Notepad").FirstOrDefault();

            IntPtr handle = process.MainWindowHandle;
            Console.WriteLine(process.ProcessName + " " + process.Id + " " + process.MachineName);
            if (handle != IntPtr.Zero)
            {
                Console.WriteLine("ok");
                SetWindowPos(handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_SHOWWINDOW);
            }
        }
    }
}
