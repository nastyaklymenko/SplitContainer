using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace SplitContainer_Test
{
    public partial class Form1 : Form
    {
        //відвіданні адреси
        ArrayList Adresses = new ArrayList();
        //індекс поточної адреси
        int currIndex = -1;
        //поточна адреса
        string currListViewAdress = "";
        public Form1()
        {
            InitializeComponent();
            //додаємо колонки
            listView1.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
            ColumnHeader c = new ColumnHeader();
            c.Text = "Iм'я";
            c.Width = c.Width + 80;
            ColumnHeader c2 = new ColumnHeader();
            c2.Text = "Розмiр";
            c2.Width = c2.Width + 60;
            ColumnHeader c3 = new ColumnHeader();
            c3.Text = "Тип";
            c3.Width = c3.Width + 60;
            ColumnHeader c4 = new ColumnHeader();
            c4.Text = "Змiнено";
            c4.Width = c4.Width + 60;
            listView1.Columns.Add(c);
            listView1.Columns.Add(c2);
            listView1.Columns.Add(c3);
            listView1.Columns.Add(c4);
            listView2.View = View.List;

            //заповнення TreeView логічними дисками і папками цих дисків
            string[] str = Environment.GetLogicalDrives();
            int n = 1;
            foreach (string s in str)
            {
                try
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = s;
                    tn.Text = "Локальный диск " + s;
                    treeView1.Nodes.Add(tn.Name, tn.Text, 2);

                    listView2.Items.Add(tn.Name);
                    listView2.Items[n - 1].ImageIndex = 2;
                    FileInfo f = new FileInfo(@s);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@s);
                    foreach (string s2 in str2)
                    {
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        ((TreeNode)treeView1.Nodes[n - 1]).Nodes.Add(s2, t, 0);
                    }
                }
                catch { }
                n++;
            }
            foreach (TreeNode tn in treeView1.Nodes)
            {
                for (int i = 65; i < 91; i++)
                {
                    char sym = Convert.ToChar(i);
                    if (tn.Name == sym + ":\\")
                        tn.SelectedImageIndex = 2;
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string strtmp = "";
            if (Adresses.Count != 0)
            {
                strtmp = ((string)Adresses[Adresses.Count - 1]);
                Adresses.Clear();
                Adresses.Add(strtmp);
                currIndex = 0;
            }
            Adresses.Add(e.Node.Name);
            currIndex++;
            //чи можемо рухатись вперед назад
            if (currIndex + 1 == Adresses.Count)
                toolStripButton2.Enabled = false;
            else
                toolStripButton2.Enabled = true;
            if (currIndex - 1 == -1)
                toolStripButton1.Enabled = false;
            else
                toolStripButton1.Enabled = true;
            listView1.Items.Clear();
            currListViewAdress = e.Node.Name;
            toolStripTextBox1.Text = currListViewAdress;
            
            //заповнюєм ліствю
            try
            {
                if (listView1.View != View.Tile)
                {
                    FileInfo f = new FileInfo(@e.Node.Name);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@e.Node.Name);
                    ListViewItem lw = new ListViewItem();
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    str2 = Directory.GetFiles(@e.Node.Name);
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                else
                {
                    FileInfo f = new FileInfo(@e.Node.Name);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@e.Node.Name);
                    ListViewItem lw = new ListViewItem();
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    str2 = Directory.GetFiles(@e.Node.Name);
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
            }
            catch { }

        }

        private void icoListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
        }

        private void imgListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }

        private void platToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Tile;
            listView1.Items.Clear();
            FileInfo f = new FileInfo(@currListViewAdress);
            string t = "";
            string[] str2 = Directory.GetDirectories(@currListViewAdress);
            ListViewItem lw = new ListViewItem();
            foreach (string s2 in str2)
            {
                f = new FileInfo(@s2);
                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                lw = new ListViewItem(new string[] { t }, 0);
                lw.Name = s2;
                listView1.Items.Add(lw);
            }
            str2 = Directory.GetFiles(@currListViewAdress);
            foreach (string s2 in str2)
            {
                f = new FileInfo(@s2);
                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                lw = new ListViewItem(new string[] { t }, 1);
                lw.Name = s2;
                listView1.Items.Add(lw);
            }
        }

        private void listToolStripMenu(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }

        private void tableToolStripMenu(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Items.Clear();
            FileInfo f = new FileInfo(@currListViewAdress);
            string t = "";
            string[] str2 = Directory.GetDirectories(@currListViewAdress);
            ListViewItem lw = new ListViewItem();
            foreach (string s2 in str2)
            {
                f = new FileInfo(@s2);
                string type = "Папка";
                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                lw.Name = s2;
                listView1.Items.Add(lw);
            }
            str2 = Directory.GetFiles(@currListViewAdress);
            foreach (string s2 in str2)
            {
                f = new FileInfo(@s2);
                string type = "Файл";
                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                lw.Name = s2;
                listView1.Items.Add(lw);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //даблклік на ліствю
            if (listView1.SelectedItems[0].Text.IndexOf('.') == -1)
            {
                //клік на папку
                Adresses.Add(listView1.SelectedItems[0].Name);
                currIndex++;
                currListViewAdress = ((string)Adresses[currIndex]);
                if (currIndex + 1 == Adresses.Count)
                    toolStripButton2.Enabled = false;
                else
                    toolStripButton2.Enabled = true;
                if (currIndex - 1 == -1)
                    toolStripButton1.Enabled = false;
                else
                    toolStripButton1.Enabled = true;
                currListViewAdress = listView1.SelectedItems[0].Name;
                toolStripTextBox1.Text = currListViewAdress;
                FileInfo f = new FileInfo(@listView1.SelectedItems[0].Name);
                string t = "";
                string[] str2 = Directory.GetDirectories(@listView1.SelectedItems[0].Name);
                string[] str3 = Directory.GetFiles(@listView1.SelectedItems[0].Name);
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();
                if (listView1.View == View.Details)
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                else
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
            }
            else
            {
                //клік на файл
                System.Diagnostics.Process MyProc = new System.Diagnostics.Process();
                MyProc.StartInfo.FileName = @listView1.SelectedItems[0].Name;
                MyProc.Start();
            }
        }
        private void ClickOnColumn(object sender, ColumnClickEventArgs e)
        {
            //зімна порядку сортування
            if (e.Column == 0)
            {
                if (listView1.Sorting == SortOrder.Descending)
                    listView1.Sorting = SortOrder.Ascending;
                else
                    listView1.Sorting = SortOrder.Descending;
            }
        }

        private void refrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Refresh();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            int i = 0;
            //заповнення дочірніх вузлів вузла
            try
            {
                foreach (TreeNode tn in e.Node.Nodes)
                {
                    string[] str2 = Directory.GetDirectories(@tn.Name);
                    foreach (string str in str2)
                    {
                        TreeNode temp = new TreeNode();
                        temp.Name = str;
                        temp.Text = str.Substring(str.LastIndexOf('\\') + 1);
                        e.Node.Nodes[i].Nodes.Add(temp);
                    }
                    i++;
                }
            }
            catch { }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //клік назад
            if (currIndex - 1 != -1)
            {
                currIndex--;
                currListViewAdress = ((string)Adresses[currIndex]);
                if (currIndex + 1 == Adresses.Count)
                    toolStripButton2.Enabled = false;
                else
                    toolStripButton2.Enabled = true;
                if (currIndex - 1 == -1)
                    toolStripButton1.Enabled = false;
                else
                    toolStripButton1.Enabled = true;
                toolStripTextBox1.Text = currListViewAdress;
                FileInfo f = new FileInfo(@currListViewAdress);
                string t = "";
                string[] str2 = Directory.GetDirectories(@currListViewAdress);
                string[] str3 = Directory.GetFiles(@currListViewAdress);
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();
                if (listView1.View == View.Details)
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                else
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //клік вперед
            if (currIndex + 1 != Adresses.Count)
            {
                currIndex++;
                currListViewAdress = ((string)Adresses[currIndex]);
                if (currIndex + 1 == Adresses.Count)
                    toolStripButton2.Enabled = false;
                else
                    toolStripButton2.Enabled = true;
                if (currIndex - 1 == -1)
                    toolStripButton1.Enabled = false;
                else
                    toolStripButton1.Enabled = true;
                toolStripTextBox1.Text = currListViewAdress;
                FileInfo f = new FileInfo(@currListViewAdress);
                string t = "";
                string[] str2 = Directory.GetDirectories(@currListViewAdress);
                string[] str3 = Directory.GetFiles(@currListViewAdress);
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();
                if (listView1.View == View.Details)
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                else
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
            }
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //перевірка чи нажали ентер і чи адреса в рядку правильна
            if (e.KeyValue == 13)
            {
                try
                {
                    string[] str2 = Directory.GetDirectories(@toolStripTextBox1.Text);
                    string[] str3 = Directory.GetFiles(@toolStripTextBox1.Text);

                   // var fileList = new DirectoryInfo(@"C:\").GetFiles(@toolStripTextBox1.Text, SearchOption.AllDirectories);

                    currIndex++;
                    currListViewAdress = toolStripTextBox1.Text;
                    Adresses.Add(toolStripTextBox1.Text);
                    if (currIndex + 1 == Adresses.Count)
                        toolStripButton2.Enabled = false;
                    else
                        toolStripButton2.Enabled = true;
                    if (currIndex - 1 == -1)
                        toolStripButton1.Enabled = false;
                    else
                        toolStripButton1.Enabled = true;
                    FileInfo f = new FileInfo(@toolStripTextBox1.Text);
                    string t = "";
                    listView1.Items.Clear();
                    ListViewItem lw = new ListViewItem();
                    if (listView1.View == View.Details)
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            string type = "Папка";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            string type = "Файл";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                    else
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                }
                catch
                {
                    toolStripTextBox1.Text = currListViewAdress;
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //вгору
            int lio = toolStripTextBox1.Text.LastIndexOf('\\');
            if (lio != -1)
            {
                toolStripTextBox1.Text = toolStripTextBox1.Text.Substring(0, lio);
                try
                {
                    string[] str2 = Directory.GetDirectories(@toolStripTextBox1.Text + "\\");
                    string[] str3 = Directory.GetFiles(@toolStripTextBox1.Text + "\\");
                    currIndex--;
                    currListViewAdress = toolStripTextBox1.Text;
                    if (currIndex + 1 == Adresses.Count)
                        toolStripButton2.Enabled = false;
                    else
                        toolStripButton2.Enabled = true;
                    if (currIndex - 1 == -1)
                        toolStripButton1.Enabled = false;
                    else
                        toolStripButton1.Enabled = true;
                    FileInfo f = new FileInfo(@toolStripTextBox1.Text + "\\");
                    string t = "";
                    listView1.Items.Clear();
                    ListViewItem lw = new ListViewItem();
                    if (listView1.View == View.Details)
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            string type = "Папка";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            string type = "Файл";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                    else
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                }
                catch
                {
                    toolStripTextBox1.Text = currListViewAdress;
                }
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            //дабл клік по логічному диску
            listView1.Items.Clear();
            currListViewAdress = listView2.SelectedItems[0].Text ;
            toolStripTextBox1.Text = currListViewAdress;

            
            try
                {
                    if (listView1.View != View.Tile)
                    {
                        FileInfo f = new FileInfo(listView2.SelectedItems[0].Text);
                        string t = "";
                        string[] str2 = Directory.GetDirectories(listView2.SelectedItems[0].Text);
                        ListViewItem lw = new ListViewItem();
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            string type = "Папка";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        str2 = Directory.GetFiles(listView2.SelectedItems[0].Text);
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            string type = "Файл";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                    else
                    {
                        FileInfo f = new FileInfo(listView2.SelectedItems[0].Text);
                        string t = "";
                        string[] str2 = Directory.GetDirectories(listView2.SelectedItems[0].Text);
                        ListViewItem lw = new ListViewItem();
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        str2 = Directory.GetFiles(listView2.SelectedItems[0].Text);
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                }
                catch { }
            }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (textBox1.Text == "")
                {
                    try
                    {
                        if (listView1.View != View.Tile)
                        {
                            FileInfo f = new FileInfo(@treeView1.SelectedNode.Name);
                            string t = "";
                            string[] str2 = Directory.GetDirectories(@treeView1.SelectedNode.Name);
                            ListViewItem lw = new ListViewItem();
                            listView1.Items.Clear();
                            foreach (string s2 in str2)
                            {
                                f = new FileInfo(@s2);
                                string type = "Папка";
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                            str2 = Directory.GetFiles(@treeView1.SelectedNode.Name);
                            foreach (string s2 in str2)
                            {
                                f = new FileInfo(@s2);
                                string type = "Файл";
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                        }
                        else
                        {
                            FileInfo f = new FileInfo(@treeView1.SelectedNode.Name);
                            string t = "";
                            string[] str2 = Directory.GetDirectories(@treeView1.SelectedNode.Name);
                            ListViewItem lw = new ListViewItem();
                            foreach (string s2 in str2)
                            {
                                f = new FileInfo(@s2);
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t }, 0);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                            str2 = Directory.GetFiles(@treeView1.SelectedNode.Name);
                            foreach (string s2 in str2)
                            {
                                f = new FileInfo(@s2);
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t }, 1);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                        }
                    }
                    catch { }
                }

                if ("" != textBox1.Text)
                {
                    //Adresses.Add(listView1.SelectedItems[0].Name);
                    //currIndex++;
                    //currListViewAdress = ((string)Adresses[currIndex]);
                    //if (currIndex + 1 == Adresses.Count)
                    //    toolStripButton2.Enabled = false;
                    //else
                    //    toolStripButton2.Enabled = true;
                    //if (currIndex - 1 == -1)
                    //    toolStripButton1.Enabled = false;
                    //else
                    //    toolStripButton1.Enabled = true;
                    //currListViewAdress = listView1.SelectedItems[0].Name;
                    //toolStripTextBox1.Text = currListViewAdress;

                    ApplyAllFiles(@currListViewAdress,textBox1.Text);
                    
                    //currIndex--;
                    currListViewAdress = toolStripTextBox1.Text;
                    if (currIndex + 1 == Adresses.Count)
                        toolStripButton2.Enabled = false;
                    else
                        toolStripButton2.Enabled = true;
                    if (currIndex - 1 == -1)
                        toolStripButton1.Enabled = false;
                    else
                        toolStripButton1.Enabled = true;
                    FileInfo f = new FileInfo(@toolStripTextBox1.Text + "\\");
                    string t = "";
                    listView1.Items.Clear();

                    ListViewItem lw = new ListViewItem();
                    if (listView1.View == View.Details)
                    {
                        foreach (string s2 in strDir)
                        {
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            if (t.Contains(textBox1.Text))
                            {
                                f = new FileInfo(@s2);
                                string type = "Папка";
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                        }
                        foreach (string s2 in strFile)
                        {
                            if (s2.Contains(textBox1.Text))
                            {
                                f = new FileInfo(@s2);
                                string type = "Файл";
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                        }
                    }
                    else
                    {
                        foreach (string s2 in strDir)
                        {
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            if (t.Contains(textBox1.Text))
                            {
                                f = new FileInfo(@s2);
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t }, 0);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                        }
                        foreach (string s2 in strFile)
                        {
                            if (s2.Contains(textBox1.Text))
                            {
                                f = new FileInfo(@s2);
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                lw = new ListViewItem(new string[] { t }, 1);
                                lw.Name = s2;
                                listView1.Items.Add(lw);
                            }
                        }
                    }
                    strDir.Clear();
                    strFile.Clear();
                }
            }
            catch { toolStripTextBox1.Text = currListViewAdress; }
            textBox1.Text = "";
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {

        }

       static List<string> strDir = new List<string>();
       static List<string> strFile = new List<string>();

        static void ApplyAllFiles(string folder, String matchtext)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                if(file.Contains(matchtext))
                strFile.Add(file);
            }
            foreach (string subDir in Directory.GetDirectories(folder))
            {
                
                try
                {
                   
                        strDir.Add(subDir);
                    ApplyAllFiles(subDir,matchtext);
                }
                catch
                {
                    // swallow, log, whatever
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    }
    

