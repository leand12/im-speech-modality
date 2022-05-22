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
using System.Collections.Generic;
using System.Windows.Threading;

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
        // LifeCycleEvents(string source, string target, string id, string medium, string mode;
        private static LifeCycleEvents lce = new LifeCycleEvents("APP", "TTS", "User1", "na", "command"); 
        private static MmiCommunication mmic = new MmiCommunication("localhost", 8000, "User1", "GUI");


        private string last = "";

        // controllers
        private SoundController soundController;
        private BrightnessController brightnessController;
        private FileExplorerController fileExplorerController;
        private TerminalController terminalController;
        private VSCodeController vsCodeController;

        public static int currentWorkspace = 0;
        public static int nWorkspaces = 1;

        public static List<WorkspaceController> workspaces = new List<WorkspaceController>();

        public MainWindow()
        {
            InitializeComponent();

            soundController = new SoundController();
            brightnessController = new BrightnessController();
            fileExplorerController = new FileExplorerController();
            terminalController = new TerminalController();
            vsCodeController = new VSCodeController();
            workspaces.Add(new WorkspaceController());

            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();
           
        }

        private string getFile(Int32 handle)
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

        private string[] GetShortcuts()
        {
            return new string[] {
                shortcut1.ToString().Replace("System.Windows.Controls.TextBox", "").Replace(": ", ""),
                shortcut2.ToString().Replace("System.Windows.Controls.TextBox", "").Replace(": ", ""),
                shortcut3.ToString().Replace("System.Windows.Controls.TextBox", "").Replace(": ", ""),
                shortcut4.ToString().Replace("System.Windows.Controls.TextBox", "").Replace(": ", ""),
                shortcut5.ToString().Replace("System.Windows.Controls.TextBox", "").Replace(": ", ""),
            };
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
            

            WorkspaceController cws = MainWindow.workspaces[MainWindow.currentWorkspace];

            Console.WriteLine(command.Target);
            Console.WriteLine(command.Action);
            Console.WriteLine(command.Value);
            
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

                case "FILE_EXPLORER":
                    fileExplorerController.Execute(command.Action, command.Value, GetShortcuts());
                    break;

                case "TERMINAL":
                    terminalController.Execute(command.Action, command.Value, GetShortcuts());
                    break;

                case "VSCODE":
                    vsCodeController.Execute(command.Action, command.Value, GetShortcuts());
                    break;

                case "WORKSPACE":
                    cws.Execute(command.Action, command.Value);
                    break;

                default:
                    if (!cws.ExecuteApp(command.Target, command.Action, command.Value))
                    {
                        Send("não consigo fazer o comando");
                    }
                    break;
            }

            mmic.Send(lce.NewContextRequest());
        }


        public static void Send(string text)
        { 
            var exNot = lce.ExtensionNotification(0 + "", 0 + "", 1, text);
            mmic.Send(exNot);
        }
    }
}
