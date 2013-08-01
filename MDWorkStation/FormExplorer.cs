using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MDWorkStation
{
    public partial class FormExplorer : Form
    {

        private string m_currentDir;

        public FormExplorer()
        {
            InitializeComponent();

            InitCtrlPos();

            PopulateTreeView();
        }

        private void InitCtrlPos()
        {
            splitContainer1.SplitterDistance = this.FindForm().Width * 1 / 4;
            splitContainer1.IsSplitterFixed = true;

            button4.Left = this.Right - button4.Width - 10;
            button4.Top = this.Top  + 15;
        }

        private void PopulateTreeView()
        {

            TreeNode rootNode;

            m_currentDir = System.Environment.CurrentDirectory + "\\Data";

            DirectoryInfo info = new DirectoryInfo(m_currentDir);


            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);

                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(rootNode);
            }
        }

        

        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode newSelected = e.Node;
                listView1.Items.Clear();
                DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;

                m_currentDir = nodeDirInfo.FullName;

                ListViewItem.ListViewSubItem[] subItems;
                ListViewItem item = null;

                foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
                {
                    item = new ListViewItem(dir.Name, 0);
                    subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, "文件夹"), 
                        new ListViewItem.ListViewSubItem(item, 
						dir.CreationTime.ToShortDateString()),
                     new ListViewItem.ListViewSubItem(item, 
						dir.LastWriteTime.ToShortDateString())};
                    item.SubItems.AddRange(subItems);
                    listView1.Items.Add(item);
                }
                foreach (FileInfo file in nodeDirInfo.GetFiles())
                {
                    item = new ListViewItem(file.Name, 1);
                    subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "文件"), 
                        new ListViewItem.ListViewSubItem(item, 
						file.CreationTime.ToString()),
                     new ListViewItem.ListViewSubItem(item, 
						file.LastWriteTime.ToString())};

                    item.SubItems.AddRange(subItems);
                    listView1.Items.Add(item);
                }

                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception)
            {
                
                //throw;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(listView1.SelectedItems[0].SubItems[0].Text);
                //除文件夹都可以打开
                if (!listView1.SelectedItems[0].SubItems[1].Text.Equals("文件夹"))
                    Process.Start(m_currentDir + "\\" + listView1.SelectedItems[0].SubItems[0].Text);
            }
            catch (System.Exception)
            {
                
            }
           

        }

        //回根目录
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PopulateTreeView();
        }
        
        //返回上级目录
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent != null)
                {
                    treeView1.SelectedNode = treeView1.SelectedNode.Parent;

                    treeView1_NodeMouseClick(this, new TreeNodeMouseClickEventArgs(treeView1.SelectedNode,MouseButtons.Left,1,0,0));
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
