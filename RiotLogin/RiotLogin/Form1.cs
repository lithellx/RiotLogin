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

        const byte VK_TAB = 0x09, VK_ENTER = 0x0D, VK_SPACE = 0x20, KEYEVENTF_KEYUP = 0x02;

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

        private void LoadData()
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Items.Clear();
            }

            string data = File.ReadAllText("info.json");
            JObject obj = JObject.Parse(data);
            bool rememberme = (bool)obj["RememberMe"];

            if (rememberme == true)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.FullRowSelect = true;
            listView1.MultiSelect = false;
            
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string username = listView1.SelectedItems[0].SubItems[1].Text;
                string password = listView1.SelectedItems[0].SubItems[2].Text;

                FocusProcess("RiotClientUx");

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

                    keybd_event(VK_TAB, 0, 0, 0);
                    keybd_event(VK_TAB, 0, KEYEVENTF_KEYUP, 0);

                    SendKeys.Send(password);

                    keybd_event(VK_ENTER, 0, 0, 0);
                    keybd_event(VK_ENTER, 0, KEYEVENTF_KEYUP, 0);
                }
            }
            else
                MessageBox.Show("Select account info to log in.", "RiotLogin", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You will see your password on this screen. Do you want to continue?", "RiotLogin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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