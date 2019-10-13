using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.IO;
using System.ServiceModel;
using Sbn.Products.SVN.SVNClient.RepositoryService;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using DiffCalc;

namespace Sbn.Products.SVN.SVNClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       // FileExplorerComponent.FileExplorer fe = null;
        //System.Windows.Controls.TreeView _treeView;

		public MainWindow()
        {
            InitializeComponent();
        }


        void AddFileExplorerToParent(DockPanel grd)
        {
          //  System.Windows.Forms.Integration.WindowsFormsHost host =  new System.Windows.Forms.Integration.WindowsFormsHost();

            // Create the MaskedTextBox control.
        //    fe = new FileExplorerComponent.FileExplorer();

            // Assign the MaskedTextBox control as the host control's child.
       //     host.Child = fe;
            // Add the interop host control to the Grid
            // control's collection of child controls.
			/*
            _treeView = new System.Windows.Controls.TreeView();
            grd.Children.Add(_treeView);
            //_treeView.p

            //fe.MouseClick += fe_MouseClick; ;
//            fe.Controls.Find("TreeView", true)[0].MouseClick += new System.Windows.Forms.MouseEventHandler(FExp_Tree_MouseClick2);
            TreeViewItem item = new TreeViewItem { Header=this.txtPath.Text};
            item.Tag = this.txtPath.Text;
            _treeView.Items.Add(item);
            _treeView.Name = "treeFiles";
            _treeView.SelectedItemChanged += tr_SelectedItemChanged;
			*/
           // fe.Dock = DockStyle.Fill;

        }

        void tr_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            System.Windows.Controls.TreeView tr = (System.Windows.Controls.TreeView)sender;
            if (tr.SelectedItem == null) return;


            if (((TreeViewItem)tr.SelectedItem).Items.Count <= 0)
            {
                string sPath = ((TreeViewItem)tr.SelectedItem).Tag.ToString()+ "\\";
                try
                {
                    foreach (string directory in Directory.GetDirectories(sPath))
                    {
                        TreeViewItem item = new TreeViewItem { Header = directory.Replace(sPath ,  "") };
                        item.Tag = directory;
                        ((TreeViewItem)tr.SelectedItem).Items.Add(item);
                    }
                }
                catch
                {

                }
            }
        }

        void fe_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }



        /*
        private void FExp_Tree_MouseClick2(object sender, System.Windows.Forms.MouseEventArgs e)
        {
         //   this.rchProjectFiles.Clear();
            lstChangedFiles.Items.Clear();
            if (fe.SelectedDirectoryName() != "")
            {
                ImageList temp = new ImageList();
                for (int i = 0; i < fe.SelectedDirectoryFiles().Length; i++)
                {
                    FileInfo fi = new FileInfo(fe.SelectedDirectoryFiles()[i]);
                    if (fi.Name.Contains(".csproj"))
                    {
                        List<string> projectFiles = new List<string>();

                        string[] sProject = File.ReadAllLines(fi.FullName);
                        foreach (string s in sProject)
                        {



                           // this.rchProjectFiles.Text += s + "\n";
                            if (s.Contains("Include=\""))
                            {
                                string sfilename = s.Substring(s.IndexOf("=\"") + 2, s.IndexOf("\"", s.IndexOf("=\"") + 2) - s.IndexOf("=\"") - 2);

                                if (!sProjectFilesExclude.Any(f => sfilename.Contains(f)))
                                {
                                    string sprjFile = System.IO.Path.GetDirectoryName(fi.FullName) + "\\" + sfilename;

                                    projectFiles.Add(s);

                                    FileInfo fil = new FileInfo(sprjFile);

                                   // temp.Images.Add(Icon.ExtractAssociatedIcon(sprjFile));
                                   // lstChangedFiles.SmallImageList = temp;
                                     lstChangedFiles.Items.Add(s);

                                }

                            }
                        }


                        break;
                    }
                }
            }
        }
        */

        private void AppendText(System.Windows.Controls.RichTextBox box, string text, Brush color)
        {
            var bc = new BrushConverter();
            bc.ConvertFromString(color.ToString());
            box.SelectionBrush = color;

            TextRange textRange = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            textRange.Text = text;
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

			if ( ((ProjectVersion) (((ListBoxItem)this.cmbProjectVersion.SelectedItem).Tag)).ProjectAccessright.Accessright == Accessright.Read)
			{
				System.Windows.MessageBox.Show("you dont have enough accessright for upload this project branch");
				return;

			}

			//System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
			System.Windows.Forms.FolderBrowserDialog fb = new FolderBrowserDialog();

			if (this.cmbBranches.SelectedItem != null)
			{
				var branchPath = ((ListBoxItem)this.cmbBranches.SelectedItem).Tag.ToString();
				fb.SelectedPath = branchPath;
			}

			if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

                if (File.Exists(fb.SelectedPath + "\\" + Tool.BranchInfo))
                {
                    string[] sbranch = File.ReadAllLines(fb.SelectedPath + "\\" + Tool.BranchInfo);
                    this.cmbBranches.Text = sbranch[0].Split(';')[1];
                    this.txtDeveloperName.Text = sbranch[0].Split(';')[0];
                }
                else
                {
                    string[] slines = new string[1];
                    slines[0] = this.txtDeveloperName.Text + ";" + this.cmbBranches.Text ;
                    File.WriteAllLines(fb.SelectedPath + "\\" + Tool.BranchInfo, slines);
                }
				var projectVersion = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[1];
				var projectName = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[0];

				var repositoryDir = fb.SelectedPath;

				UploadBranchFiles._repositoryDir = repositoryDir;
				UploadBranchFiles._projectName = projectName;
				UploadBranchFiles._projectVersion = projectVersion;
				UploadBranchFiles._branchName = this.cmbBranches.Text;
				UploadBranchFiles._develpoerName = this.txtDeveloperName.Text;
				UploadBranchFiles._getType = "upload";

				new UploadBranchFiles().ShowDialog(); 
			}



        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #region Regedit_OldCon
        private bool RemoveRegistry()
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("SbnSVN", true);
            if (FtpClient == null)
            {
                System.Windows.Forms.MessageBox.Show("Error! Can't Remove Old Connections");
                return false;
            }
            else
            {
                soft.DeleteSubKeyTree("FTPClient");
                return true;
            }
        }
        private void WriteRegistry(string VersionID, string FTPServer, string DeveloperName, string Password, string BranchName)
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("SbnSVN", true);
            RegistryKey yni = null;
            if (FtpClient == null)
            {
                yni = soft.CreateSubKey("SbnSVN");
                FtpClient = yni;
            }
            bool found = false;

            for (int i = 0; i < FtpClient.SubKeyCount; i++)
            {
                RegistryKey Info = FtpClient.OpenSubKey(i.ToString(), true);

                if (Info != null && Info.GetValue("VersionID").ToString() == VersionID)
                {
                    found = true;
                    RegistryKey Con = Info;
                    Con.SetValue("VersionID", VersionID);
                    Con.SetValue("Server", FTPServer);
                    Con.SetValue("DeveloperName", DeveloperName);
                    Con.SetValue("Password", Password);
                    Con.SetValue("BranchName", BranchName);
                }
            }
            if(!found)
            {
                RegistryKey Con = FtpClient.CreateSubKey(VersionID);
                Con.SetValue("VersionID", VersionID);
                Con.SetValue("Server", FTPServer);
                Con.SetValue("DeveloperName", DeveloperName);
                Con.SetValue("Password", Password);
                Con.SetValue("BranchName", BranchName);
            }

        }
        private int OldConCount()
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
                return 0;
            return FtpClient.SubKeyCount;
        }
        //private bool ControlForMore(string FTPServer)
        //{
        //    RegistryKey root = Registry.LocalMachine;
        //    RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
        //    RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
        //    if (FtpClient == null)
        //        return true;
        //    for (int i = 0; i < FtpClient.SubKeyCount; i++)
        //    {
        //        RegistryKey Info = FtpClient.OpenSubKey(i.ToString(), true);
        //        if (Server == Info.GetValue("Server").ToString())
        //            return false;
        //    }
        //    return true;
        //}
        private string ReadServerNamesRegistry(string Number)
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
                return "";
            RegistryKey Info = FtpClient.OpenSubKey(Number, true);
            return Info.GetValue("Server").ToString();
        }
        private string ReadServerInfo(string VersionID)
        {
            string ServerInfoRegedit = "";
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("SbnSVN", true);
            if (FtpClient == null)
                return ServerInfoRegedit;
            for (int i = 0; i < FtpClient.SubKeyCount; i++)
            {
                RegistryKey Info = FtpClient.OpenSubKey(i.ToString(), true);
                if (Info != null && Info.GetValue("VersionID").ToString() == VersionID)
                {
                    ServerInfoRegedit += Info.GetValue("DeveloperName").ToString() + ",";
                    ServerInfoRegedit += Info.GetValue("Password").ToString() + ",";
                    ServerInfoRegedit += Info.GetValue("BranchName").ToString() + ",";
                }
            }
            return ServerInfoRegedit;
        }
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
				/*
            using (var ch = new RepositoryService.RepositoryServiceSoapClient())
            {
                var bts = ch.GetProjectBranchState(int.Parse(((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString()), this.txtBranchName.Text, this.txtDeveloperName.Text);

                List<string> lines = new List<string>();

                foreach (BranchFile bf in bts)
                {
                    lines.Add( bf.FilePath + ";" + bf.LastEditionDate.ToString());
                }
				if (_treeView.SelectedItem == null)
				{
					System.Windows.MessageBox.Show("محل ذخیره سازی پروژه را تعیین نمایید!");
					return;
				}
				string item = ((TreeViewItem)_treeView.SelectedItem).Tag.ToString();
				File.WriteAllLines(item + "\\ProjectBranchState.stat", lines.ToArray());

            }
			  */
        }


		public static DialogResult InputBox(string title, string promptText, ref string value , ref string pass)
		{
			Form form = new Form();
			System.Windows.Forms.Label label = new  System.Windows.Forms.Label ();
			System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
			System.Windows.Forms.TextBox textBoxPass = new System.Windows.Forms.TextBox();
			System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
			System.Windows.Forms.Button buttonCancel = new  System.Windows.Forms.Button ();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;


			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			textBoxPass.UseSystemPasswordChar = true;
			textBoxPass.SetBounds(12, 70, 372, 20);

			buttonOk.SetBounds(228, 100, 75, 23);
			buttonCancel.SetBounds(309, 100, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			textBoxPass.Anchor = textBoxPass.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new System.Drawing.Size (396, 150);
			form.Controls.AddRange(new System.Windows.Forms.Control[] { label, textBox , textBoxPass, buttonOk, buttonCancel });
			form.ClientSize = new System.Drawing.Size (Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;
			form.RightToLeft = RightToLeft.Yes;


			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

			string user = ""; string pass = "";
			if (MainWindow.InputBox("Enter your name and password!", "", ref user, ref pass) != System.Windows.Forms.DialogResult.OK)
			{
				this.Close();
				
				return; 
			}
			this.txtDeveloperName.Text = user;

            Tool.ProjectFilesExclude = global::Sbn.Products.SVN.SVNClient.Properties.Settings.Default.sProjectFilesExclude.Split(',').ToList();//  new string[] { ".dll", ".exe", ".pdb", ".cache", ".resources" };
			Tool.ProjectFilesExtention = global::Sbn.Products.SVN.SVNClient.Properties.Settings.Default.sProjectFilesExtention.Split(',').ToList();//new string[] { ".csproj" };
			Tool.ProjectPathsExclude = global::Sbn.Products.SVN.SVNClient.Properties.Settings.Default.sProjectPathsExclude.Split(',').ToList();//new string[] { ".csproj" };


            using (var ch = new RepositoryService.RepositoryServiceSoapClient())
            {
				Solution[] slns = ch.GetSolutions(new Developer { Name = this.txtDeveloperName.Text });
				foreach (Solution pv in slns)
				{
					this.cmbSolutions.Items.Add(new ListBoxItem { Content = pv.Name , Tag= pv.Id });
				}


				var sysDate = ch.GetServerDatetime();

				SYSTEMTIME st = new SYSTEMTIME();
				st.wYear = (short)sysDate.Year; // must be short
				st.wMonth = (short)sysDate.Month;
				st.wDay = (short)sysDate.Day;
				st.wHour = (short)sysDate.Hour;
				st.wMinute = (short)sysDate.Minute;
				st.wSecond = (short)sysDate.Second;

				SetSystemTime(ref st); // invoke this method.
            }

            
            if (this.cmbProjectVersion.SelectedItem == null) this.cmbProjectVersion.SelectedIndex = 0;
            //string vals = ReadServerInfo(((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString());
			///test ch del


        }

        private void txtPath_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {	/*
            if (e.Key == Key.Enter)
            {
                this._treeView.Items.Clear();

                TreeViewItem item = new TreeViewItem { Header = this.txtPath.Text };
                item.Tag = this.txtPath.Text;
                _treeView.Items.Add(item);
            }*/
        }

		private void getLatestVersion(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog fb = new FolderBrowserDialog ();
			try
			{
				if (this.cmbBranches.SelectedItem != null)
				{
					var branchPath = ((ListBoxItem)this.cmbBranches.SelectedItem).Tag.ToString();
					if (Directory.Exists(branchPath))
					{

					}
					else
					{
						Directory.CreateDirectory(branchPath);

					}
					fb.SelectedPath = branchPath;
				}
			}
			catch
			{

			}
			if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if (System.Windows.MessageBox.Show("The selected folder may contain more recent files from the server.\nAre You sure for write server files on Your Local Files ?", "Alert", MessageBoxButton.YesNoCancel, MessageBoxImage.Stop) == MessageBoxResult.No)
				{
					return;
				}

				var repositoryDir = fb.SelectedPath;
				if (File.Exists(repositoryDir + "\\" + Tool.BranchInfo))
				{

					string[] sbranch = File.ReadAllLines(repositoryDir + "\\" + Tool.BranchInfo);
					this.cmbBranches.Text = sbranch[0].Split(';')[1];
					this.txtDeveloperName.Text = sbranch[0].Split(';')[0];
				}
				else
				{
					string[] slines = new string[1];
					slines[0] = this.txtDeveloperName.Text + ";" + this.cmbBranches.Text;

					File.WriteAllLines(repositoryDir + "\\" + Tool.BranchInfo , slines);

					//System.Windows.Forms.MessageBox.Show("make branch information file in selected path first!");
					//return;
				}

				var projectVersion = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[1];
				var projectName = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[0];

				UploadBranchFiles._repositoryDir = repositoryDir;
				UploadBranchFiles._projectName = projectName;
				UploadBranchFiles._projectVersion = projectVersion;
				UploadBranchFiles._branchName = this.cmbBranches.Text;
				UploadBranchFiles._develpoerName = this.txtDeveloperName.Text;
				UploadBranchFiles._getType = "getbranch";

				new UploadBranchFiles().ShowDialog(); 
				
			}


		}


		private void getProjectVersion(object sender, RoutedEventArgs e)
		{

			try
			{
				System.Windows.Forms.FolderBrowserDialog fb = new FolderBrowserDialog();
				try
				{
					if (this.cmbBranches.SelectedItem != null)
					{
						var branchPath = ((ListBoxItem)this.cmbBranches.SelectedItem).Tag.ToString();
						if (Directory.Exists(branchPath))
						{

						}
						else
						{

								Directory.CreateDirectory(branchPath);


						}
						fb.SelectedPath = branchPath;
					}
				}
				catch
				{

				}

				if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					if (System.Windows.MessageBox.Show("The selected folder may contain more recent files from the server.\nAre You sure for write server files on Your Local Files ?", "Alert", MessageBoxButton.YesNoCancel, MessageBoxImage.Stop) == MessageBoxResult.No)
					{
						return;
					}
					var repositoryDir = fb.SelectedPath;
					var projectVersion = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[1];
					var projectName = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[0];
					/*
					if (File.Exists(repositoryDir + "\\" + Tool.BranchInfo))
					{

						string[] sbranch = File.ReadAllLines(repositoryDir + "\\" + Tool.BranchInfo);
						this.txtBranchName.Text = sbranch[0].Split(';')[1];
						this.txtDeveloperName.Text = sbranch[0].Split(';')[0]; 
					}
					else
					{
					
					 * 
						string[] slines = new string[1];
						slines[0] = this.txtDeveloperName.Text + ";" + this.txtBranchName.Text;

						File.WriteAllLines(repositoryDir + "\\" + Tool.BranchInfo, slines);
					
					}
					*/
					UploadBranchFiles._repositoryDir = repositoryDir;
					UploadBranchFiles._projectName = projectName;
					UploadBranchFiles._projectVersion = projectVersion;
					UploadBranchFiles._branchName = this.cmbBranches.Text;
					UploadBranchFiles._develpoerName = this.txtDeveloperName.Text;
					UploadBranchFiles._getType = "getproject";

					new UploadBranchFiles().ShowDialog();


				}
			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
			}


		}

		private void ApplyToProjectVersion(object sender, RoutedEventArgs e)
		{
			if (((ProjectVersion)(((ListBoxItem)this.cmbProjectVersion.SelectedItem).Tag)).ProjectAccessright.Accessright == Accessright.Read)
			{
				System.Windows.MessageBox.Show("you dont have enough accessright for upload this project branch");
				return;

			}

			if (System.Windows.MessageBox.Show("Are You sure for wite Your Local Files On the main version of project source on the server ?", "هشدار", MessageBoxButton.YesNoCancel, MessageBoxImage.Stop) == MessageBoxResult.No)
			{
				return;
			}


			var projectVersion = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[1];
			var projectName = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[0];

			using (var ch = new RepositoryService.RepositoryServiceSoapClient())
			{
				ch.ApplyBranchToProjectVersion(new BranchFile { Branch = new Branch { ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(projectVersion), Project = new Project { Name = projectName } }, Developer = new Developer { Name = this.txtDeveloperName.Text }, Name = this.cmbBranches.Text }, Developer = new Developer { Name = this.txtDeveloperName.Text } });
			}

/*
			System.Windows.Forms.FolderBrowserDialog fb = new FolderBrowserDialog ();



			 
			if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				var repositoryDir = fb.SelectedPath;
				if (File.Exists(repositoryDir + "\\" + Tool.BranchInfo))
				{

					string[] sbranch = File.ReadAllLines(repositoryDir + "\\" + Tool.BranchInfo);
					this.txtBranchName.Text = sbranch[0].Split(';')[1];
					this.txtDeveloperName.Text = sbranch[0].Split(';')[0];
				}
				else
				{
					string[] slines = new string[1];
					slines[0] = this.txtDeveloperName.Text + ";" + this.txtBranchName.Text;

					File.WriteAllLines(repositoryDir + "\\" + Tool.BranchInfo, slines);

					//System.Windows.Forms.MessageBox.Show("make branch information file in selected path first!");
					//return;
				}


			}
*/
		}

		private void cmbProjectVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.cmbBranches.Items.Clear();
			if (this.cmbProjectVersion.SelectedItem == null) return;
			using (var ch = new RepositoryService.RepositoryServiceSoapClient())
			{
				var projectVersion = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[1];
				var projectName = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[0];
				Branch[] pvs = ch.GetDevelopersBranch(new Branch { Developer = new Developer{ Name = this.txtDeveloperName.Text}, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(projectVersion), Project = new Project { Name = projectName } } });
				foreach (Branch pv in pvs)
				{
					this.cmbBranches.Items.Add(new ListBoxItem { Content = pv.Name, Tag = pv.LocalPath });
				}
			}
			if (this.cmbBranches.SelectedItem == null) this.cmbBranches.SelectedIndex = 0;
		}

		private void cmbBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.cmbBranches.SelectedItem != null)
			{
				var branchName = ((ListBoxItem)this.cmbBranches.SelectedItem).Content.ToString();
				this.cmbBranches.Text = branchName;
			}

		}

		#region systemDatetime

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetSystemTime(ref SYSTEMTIME st);

		[StructLayout(LayoutKind.Sequential)]
		public struct SYSTEMTIME
		{
			public short wYear;
			public short wMonth;
			public short wDayOfWeek;
			public short wDay;
			public short wHour;
			public short wMinute;
			public short wSecond;
			public short wMilliseconds;
		}
		#endregion

		private void ViewHistory(object sender, RoutedEventArgs e)
		{
			try
			{

				System.Windows.Forms.OpenFileDialog fb = new System.Windows.Forms.OpenFileDialog();
				string branchPath = "";
				if (this.cmbBranches.SelectedItem != null)
				{
					branchPath = ((ListBoxItem)this.cmbBranches.SelectedItem).Tag.ToString();
					if (Directory.Exists(branchPath))
					{

					}
					else
					{
						//	Directory.CreateDirectory(branchPath);

					}
					//fb.fo = branchPath;
				}
				if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK && branchPath != "")
				{
					List<string> filenames = new List<string>();
					using (var ch = new RepositoryService.RepositoryServiceSoapClient())
					{
						var projectVersion = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[1];
						var projectName = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[0];



						var bf = new BranchFile { FilePath = fb.FileName.Replace(branchPath, ""), Branch = new Branch { ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(projectVersion), Project = new Project { Name = projectName } } } };
						bf.DevelopmentStatus = ContentStatus.CheckedIN;

						BranchFile[] pvs = ch.GetProjectContentHistory(bf);
						foreach (BranchFile f in pvs)
						{
							filenames.Add(f.ID.ToString() + ";" + f.LastEditionDate + ";" + f.Developer.Name);
						}
					}

					DiffCalc.MainForm main = new MainForm();
					main.setSourceList(filenames.ToArray());
					main.setDestList(filenames.ToArray());

					main.Show();


				}

			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);

			}
		}

		private void cmbSolutions_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			this.cmbProjectVersion.Items.Clear();

			using (var ch = new RepositoryService.RepositoryServiceSoapClient())
			{
				ProjectVersion[] pvs = ch.GetProjects(new Developer { Name = this.txtDeveloperName.Text }, new Solution { Id = int.Parse((((ListBoxItem)this.cmbSolutions.SelectedItem).Tag.ToString()))});
				foreach (ProjectVersion pv in pvs)
				{
					this.cmbProjectVersion.Items.Add(new ListBoxItem { Content = pv.Project.Name + ";" + pv.VersionNumber , Tag = pv});
				}
			}

		}

		private void btnLocalDiffrencesSaving_Click(object sender, RoutedEventArgs e)
		{
			//System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
			System.Windows.Forms.FolderBrowserDialog fb = new FolderBrowserDialog();

			if (this.cmbBranches.SelectedItem != null)
			{
				var branchPath = ((ListBoxItem)this.cmbBranches.SelectedItem).Tag.ToString();
				fb.SelectedPath = branchPath;
			}

			if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				if (File.Exists(fb.SelectedPath + "\\" + Tool.BranchInfo))
				{
					string[] sbranch = File.ReadAllLines(fb.SelectedPath + "\\" + Tool.BranchInfo);
					this.cmbBranches.Text = sbranch[0].Split(';')[1];
					this.txtDeveloperName.Text = sbranch[0].Split(';')[0];
				}
				else
				{
					string[] slines = new string[1];
					slines[0] = this.txtDeveloperName.Text + ";" + this.cmbBranches.Text;

					File.WriteAllLines(fb.SelectedPath + "\\" + Tool.BranchInfo, slines);
				}
				var projectVersion = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[1];
				var projectName = ((ListBoxItem)this.cmbProjectVersion.SelectedItem).Content.ToString().Split(';')[0];

				var repositoryDir = fb.SelectedPath;

				UploadBranchFiles._repositoryDir = repositoryDir;
				UploadBranchFiles._projectName = projectName;
				UploadBranchFiles._projectVersion = projectVersion;
				UploadBranchFiles._branchName = this.cmbBranches.Text;
				UploadBranchFiles._develpoerName = this.txtDeveloperName.Text;
				UploadBranchFiles._getType = "offlineDiff";

				new UploadBranchFiles().ShowDialog();
			}
		}
	}
}
