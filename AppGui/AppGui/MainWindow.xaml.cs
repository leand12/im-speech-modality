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


        private string last = "";

        // controllers
        private SoundController soundController;
        private BrightnessController brightnessController;
        private ApplicationController appController;
        private WorkspaceController workspaceController;

        public MainWindow()
        {
            InitializeComponent();

            soundController = new SoundController();
            brightnessController = new BrightnessController();
            appController = new ApplicationController();
            workspaceController = new WorkspaceController();

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
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);

            // convert json to object
            Command command = new Command();
            command.Target = json.recognized.target;
            command.Action = json.recognized.action;
            command.Value = json.recognized.value;

            //Console.WriteLine(command.action);

            // if the target is "LAST", then provide the last one
            if (command.Target != "LAST")
                last = command.Target;
            else
                command.Target = last;

            switch (command.Target)
            {
                case "VOLUME":
                    //camaraController.Execute(new string[] { "OPEN" });
                    soundController.Execute(command.Action, command.Value);
                    break;

                case "BRIGHT":
                    //camaraController.Execute(new string[] { "CLOSE" });
                    brightnessController.Execute(command.Action, command.Value);
                    break;

                case "WORKSPACE":
                    workspaceController.Execute(command.Action, command.Value);
                    break;

                default:
                    appController.Execute(command.Target, command.Action, command.Value);
                    //windowController.Execute(new string[] { target, (string)json.recognized[2].ToString() });
                    //fileSystemController.Execute(command.action, command.value);
                    break;
            }


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
