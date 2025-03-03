using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;

namespace RiotLogin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow); //ShowWindow needs an IntPtr

        [DllImport("user32.dll")]
        internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //[DllImport("user32.dll", SetLastError = true)]
        //private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //IntPtr hWnd = FindWindow(null, "Riot Client");


        const byte VK_TAB = 0x09, VK_ENTER = 0x0D, VK_SPACE = 0x20, KEYEVENTF_KEYUP = 0x02;
        
        public string RiotClientProc = "Riot Client", RiotClientUxProc = "RiotClientUx"; // Riot Client & RiotClientUx
        public string RiotGamesPath = ""; // "C:\\Riot Games"
        public string RiotClientExe = "\\Riot Client\\RiotClientServices.exe"; // "\\Riot Client\\RiotClientServices.exe"

        

        private static bool isProcessA(string processName)
        {
            IntPtr hWnd; //change this to IntPtr
            Process[] processRunning = Process.GetProcesses();
            foreach (Process pr in processRunning)
            {
                if (pr.ProcessName == processName)
                {
                    return true;
                }
            }
            return false;
        }

        private static void FocusProcess(string ProcessName)
        {
            IntPtr hWnd; //change this to IntPtr
            Process[] processRunning = Process.GetProcesses();
            foreach (Process pr in processRunning)
            {
                if (pr.ProcessName == ProcessName)
                {
                    hWnd = pr.MainWindowHandle; //use it as IntPtr not int
                    ShowWindow(hWnd, 1);
                    SetForegroundWindow(hWnd); //set to topmost
                }
            }
        }

        void RunRiotClient()
        {
            Process.Start(RiotGamesPath + RiotClientExe);
        }

        private void LoadData()
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Items.Clear();
            }

            if (File.Exists("info.json"))
            {
                string data = File.ReadAllText("info.json");
                JObject obj = JObject.Parse(data);
                bool rememberMe = (bool)obj["RememberMe"];
                RiotGamesPath = (string)obj["RiotGamesPath"];

                if (rememberMe == true)
                {
                    checkBox1.Checked = true;
                }
                else
                    checkBox1.Checked = false;

                var fileName = "info.json";
                StreamReader reader = new StreamReader(fileName);
                var content = reader.ReadToEnd();
                DataTable profiles = JObject.Parse(content)["Profiles"].ToObject<DataTable>();

                foreach (DataRow row in profiles.Rows)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());
                    for (int i = 1; i < profiles.Columns.Count; i++)
                    {
                        item.SubItems.Add(row[i].ToString());
                    }

                    listView1.Items.Add(item);
                }

                reader.Close();
            }
            else
            {
                string jsonTemplate = "{\r\n  \"RememberMe\": 0,\r\n  \"RiotGamesPath\": \"C:\\\\Riot Games\",\r\n  \"Profiles\": [\r\n  ]\r\n}";
                File.WriteAllText("info.json", jsonTemplate);
                LoadData();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FocusProcess("Riot Client");

            if (!File.Exists("Newtonsoft.Json.dll"))
            {
                MessageBox.Show("RiotLogin cannot found \"Newtonsoft.Json.dll\". Please make sure you keep dll in same folder with the program.", "RiotLogin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] RiotClient = Process.GetProcessesByName(RiotClientProc);
            Process[] RiotClientUx = Process.GetProcessesByName(RiotClientUxProc);
            if (RiotClient.Length > 0 || RiotClientUx.Length > 0)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    string username = listView1.SelectedItems[0].SubItems[1].Text;
                    string password = listView1.SelectedItems[0].SubItems[2].Text;

                    if (isProcessA(RiotClientProc))
                        FocusProcess(RiotClientProc);
                    else
                        FocusProcess(RiotClientUxProc);

                    if (checkBox1.Checked == true)
                    {
                        SendKeys.Send(username);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        SendKeys.Send(password);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        keybd_event(VK_SPACE, 0, 0, 0);
                        keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, 0);

                        Thread.Sleep(100);

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        keybd_event(VK_ENTER, 0, 0, 0);
                        keybd_event(VK_ENTER, 0, KEYEVENTF_KEYUP, 0);
                    }
                    else
                    {
                        SendKeys.Send(username);
                        SendKeys.Send("%");

                        keybd_event(VK_TAB, 0, 0, 0);
                        keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                        Thread.Sleep(100);

                        SendKeys.Send(password);

                        keybd_event(VK_ENTER, 0, 0, 0);
                        keybd_event(VK_ENTER, 0, KEYEVENTF_KEYUP, 0);
                    }
                }
                else
                {
                    MessageBox.Show("Select account info to log in.", "RiotLogin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                if (File.Exists(RiotGamesPath + RiotClientExe))
                {
                    MessageBox.Show("Riot Client is being started. Please wait until it fully starts.", "RiotLogin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RunRiotClient();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("RiotLogin couldn't find Riot Games path. Would you like to select path by yourself (Yes) or open Riot Client manually (No)?", "RiotLogin", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                    {
                        FolderBrowserDialog rcpDialog = new FolderBrowserDialog();
                        rcpDialog.Description = "Select the \"Riot Games\" folder.";
                        if (rcpDialog.ShowDialog() == DialogResult.OK)
                        {
                            RiotGamesPath = rcpDialog.SelectedPath;
                            string data = File.ReadAllText("info.json");
                            dynamic obj = JsonConvert.DeserializeObject(data);
                           if (RiotGamesPath != string.Empty)
                                obj["RiotGamesPath"] = RiotGamesPath;
                            string output = JsonConvert.SerializeObject(obj, Formatting.Indented);
                            File.WriteAllText("info.json", output);
                            LoadData();
                        }
                        RunRiotClient();
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You will see your passwords on this screen. Do you want to continue?", "RiotLogin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Form2 frm2 = new Form2();
                frm2.ShowDialog();
                LoadData();
            }          
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string data = File.ReadAllText("info.json");
            dynamic obj = JsonConvert.DeserializeObject(data);
            if(checkBox1.Checked == true)
                obj["RememberMe"] = 1;
            else
                obj["RememberMe"] = 0;
            string output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText("info.json", output);
        }
    }
}

// coded by lithellx - https://github.com/lithellx
