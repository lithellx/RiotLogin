using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Diagnostics;

namespace RiotLogin
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            if (listView1.Items.Count != 0)
            {
                listView1.Items.Clear();
            }

            string data = File.ReadAllText("info.json");
            JObject obj = JObject.Parse(data);
            bool exportdata = (bool)obj["ExportData"];

            if (exportdata == true)
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

        private void Form2_Load(object sender, EventArgs e)
        {
            listView1.FullRowSelect = true;
            listView1.MultiSelect = false;

            LoadData();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && checkBox1.Checked == true)
            {
                textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
                textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBox3.Text = listView1.SelectedItems[0].SubItems[2].Text;
                Action doNothing = () => { };
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string data = File.ReadAllText("info.json");
                dynamic obj = JsonConvert.DeserializeObject(data);
                if(textBox1.Text != string.Empty)
                    obj["Profiles"][int.Parse(listView1.SelectedItems[0].Index.ToString())]["AccountTag"] = textBox1.Text;
                if (textBox2.Text != string.Empty)
                    obj["Profiles"][int.Parse(listView1.SelectedItems[0].Index.ToString())]["Username"] = textBox2.Text;
                if (textBox3.Text != string.Empty)
                    obj["Profiles"][int.Parse(listView1.SelectedItems[0].Index.ToString())]["Password"] = textBox3.Text;
                string output = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText("info.json", output);
                LoadData();
            }
            else
            {
                MessageBox.Show("Select row to edit account info.", "RiotLogin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty)
            {
                string data = File.ReadAllText("info.json");
                JObject obj = JObject.Parse(data);
                JArray array = (JArray)obj["Profiles"];
                JObject tempObj = new JObject();

                tempObj.Add("AccountTag", textBox1.Text);
                tempObj.Add("Username", textBox2.Text);
                tempObj.Add("Password", textBox3.Text);
                array.Add(tempObj);

                string output = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText("info.json", output);

                LoadData();
            }
            else
            {
                MessageBox.Show("Enter row data to add an account info.", "RiotLogin", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        public static string RemoveFirstInstanceOfString(string value, string removeString)
        {
            int index = value.IndexOf(removeString, StringComparison.Ordinal);
            return index < 0 ? value : value.Remove(index, removeString.Length);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty)
            {
                string data = File.ReadAllText("info.json");
                JObject obj = JObject.Parse(data);
                JObject tempObj = new JObject();
                JArray array = (JArray)obj["Profiles"];

                tempObj.Add("AccountTag", textBox1.Text);
                tempObj.Add("Username", textBox2.Text);
                tempObj.Add("Password", textBox3.Text);
                array.Remove(tempObj);

                string edit, edit2, result;
                if (array.Count > 1)
                {
                    edit = JsonConvert.SerializeObject(obj);
                    edit2 = "," + JsonConvert.SerializeObject(tempObj);
                    result = RemoveFirstInstanceOfString(edit, edit2);
                }
                else
                {
                    edit = JsonConvert.SerializeObject(obj);
                    edit2 = JsonConvert.SerializeObject(tempObj);
                    result = RemoveFirstInstanceOfString(edit, edit2);
                }

                JObject obj2 = JObject.Parse(result);
                string output = JsonConvert.SerializeObject(obj2, Formatting.Indented);
                File.WriteAllText("info.json", output);

                LoadData();
            }
            else
            {
                MessageBox.Show("Enter the account info you want to delete.", "RiotLogin", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            string data = File.ReadAllText("info.json");
            dynamic obj = JsonConvert.DeserializeObject(data);
            if (checkBox1.Checked == true)
                obj["ExportData"] = 1;
            else
                obj["ExportData"] = 0;
            string output = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText("info.json", output);

        }
    }
}

// coded by lithellx - https://github.com/lithellx