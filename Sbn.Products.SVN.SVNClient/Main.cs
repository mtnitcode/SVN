using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using DifferenceEngine;
using Sbn.Products.SVN.SVNClient.RepositoryService;
using Sbn.Products.SVN.SVNClient;

namespace DiffCalc
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cmdCompare;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.CheckBox chkBinary;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbFast;
		private System.Windows.Forms.RadioButton rbMedium;
		private System.Windows.Forms.RadioButton rbSlow;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Label lblFileName;
		private ComboBox cmbSrc;
		private ComboBox cmbDest;

		private DiffEngineLevel _level;

		public MainForm()
		{
			
			InitializeComponent();

			_level = DiffEngineLevel.FastImperfect; 
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmdCompare = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdClose = new System.Windows.Forms.Button();
			this.chkBinary = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbSlow = new System.Windows.Forms.RadioButton();
			this.rbMedium = new System.Windows.Forms.RadioButton();
			this.rbFast = new System.Windows.Forms.RadioButton();
			this.lblFileName = new System.Windows.Forms.Label();
			this.cmbSrc = new System.Windows.Forms.ComboBox();
			this.cmbDest = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdCompare
			// 
			this.cmdCompare.Location = new System.Drawing.Point(231, 186);
			this.cmdCompare.Name = "cmdCompare";
			this.cmdCompare.Size = new System.Drawing.Size(61, 20);
			this.cmdCompare.TabIndex = 8;
			this.cmdCompare.Text = "Compare";
			this.cmdCompare.Click += new System.EventHandler(this.cmdCompare_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Source:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "Destination:";
			// 
			// cmdClose
			// 
			this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdClose.Location = new System.Drawing.Point(298, 186);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(61, 20);
			this.cmdClose.TabIndex = 9;
			this.cmdClose.Text = "Close";
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// chkBinary
			// 
			this.chkBinary.Location = new System.Drawing.Point(20, 189);
			this.chkBinary.Name = "chkBinary";
			this.chkBinary.Size = new System.Drawing.Size(83, 16);
			this.chkBinary.TabIndex = 7;
			this.chkBinary.Text = "Binary Diff";
			this.chkBinary.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbSlow);
			this.groupBox1.Controls.Add(this.rbMedium);
			this.groupBox1.Controls.Add(this.rbFast);
			this.groupBox1.Location = new System.Drawing.Point(7, 126);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(354, 51);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Diff Level";
			// 
			// rbSlow
			// 
			this.rbSlow.Location = new System.Drawing.Point(180, 18);
			this.rbSlow.Name = "rbSlow";
			this.rbSlow.Size = new System.Drawing.Size(83, 24);
			this.rbSlow.TabIndex = 2;
			this.rbSlow.Text = "Slow/Best";
			// 
			// rbMedium
			// 
			this.rbMedium.Location = new System.Drawing.Point(89, 18);
			this.rbMedium.Name = "rbMedium";
			this.rbMedium.Size = new System.Drawing.Size(69, 24);
			this.rbMedium.TabIndex = 1;
			this.rbMedium.Text = "Medium";
			// 
			// rbFast
			// 
			this.rbFast.Checked = true;
			this.rbFast.Location = new System.Drawing.Point(10, 18);
			this.rbFast.Name = "rbFast";
			this.rbFast.Size = new System.Drawing.Size(45, 24);
			this.rbFast.TabIndex = 0;
			this.rbFast.TabStop = true;
			this.rbFast.Text = "Fast";
			// 
			// lblFileName
			// 
			this.lblFileName.AutoSize = true;
			this.lblFileName.Location = new System.Drawing.Point(6, 9);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(46, 13);
			this.lblFileName.TabIndex = 10;
			this.lblFileName.Text = "filename";
			// 
			// cmbSrc
			// 
			this.cmbSrc.FormattingEnabled = true;
			this.cmbSrc.Location = new System.Drawing.Point(9, 48);
			this.cmbSrc.Name = "cmbSrc";
			this.cmbSrc.Size = new System.Drawing.Size(352, 21);
			this.cmbSrc.TabIndex = 11;
			// 
			// cmbDest
			// 
			this.cmbDest.FormattingEnabled = true;
			this.cmbDest.Location = new System.Drawing.Point(9, 96);
			this.cmbDest.Name = "cmbDest";
			this.cmbDest.Size = new System.Drawing.Size(352, 21);
			this.cmbDest.TabIndex = 12;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdClose;
			this.ClientSize = new System.Drawing.Size(371, 225);
			this.Controls.Add(this.cmbDest);
			this.Controls.Add(this.cmbSrc);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.chkBinary);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdCompare);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Compare Files";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//[STAThread]
		/*static void Main() 
		{
			Application.Run(new MainForm());
		}*/

		private string GetFileName()
		{
			string fname = string.Empty;
			OpenFileDialog dlg = new OpenFileDialog();

			dlg.InitialDirectory = "c:\\" ;
			dlg.Filter = "All files (*.*)|*.*" ;
			dlg.FilterIndex = 1 ;
			dlg.RestoreDirectory = true ;

			if(dlg.ShowDialog() == DialogResult.OK)
			{
				fname = dlg.FileName;
			}
			return fname;
		}

		private void cmdSource_Click(object sender, System.EventArgs e)
		{
			//txtSource.Text = GetFileName();
		}

		private void cmdDestination_Click(object sender, System.EventArgs e)
		{
			//txtDestination.Text = GetFileName();
		}

		private void cmdClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private bool ValidFile(string fname)
		{
			if (fname != string.Empty)
			{
				if (File.Exists(fname))
				{
					return true;
				}
			}
			return false;
		}

		private void TextDiff(string sFile, string dFile)
		{
			this.Cursor = Cursors.WaitCursor;

			DiffList_TextFile sLF = null;
			DiffList_TextFile dLF = null;
			try
			{

				using (var ch = new Sbn.Products.SVN.SVNClient.RepositoryService.RepositoryServiceSoapClient())
				{

					long sFileID = long.Parse( sFile.Split(';')[0]);
					long dFileID = long.Parse( dFile.Split(';')[0]);

					var bf = new BranchFile { FilePath="" , ID =  sFileID };
					bf.DevelopmentStatus = ContentStatus.CheckedIN;

					BranchFile pvs = ch.DownloadBranchContent(bf);

					byte[] bSource = Tool.DecompressContent(pvs.FileContent);
					string sSource = System.Text.Encoding.UTF8.GetString(bSource);
					string sSources = sSource;//.Split('\r');
					sLF = new DiffList_TextFile(sSources);

					bf = new BranchFile { FilePath = "", ID = dFileID };
					bf.DevelopmentStatus = ContentStatus.CheckedIN;

					pvs = ch.DownloadBranchContent(bf);

					bSource = Tool.DecompressContent(pvs.FileContent);
					string dSource = System.Text.Encoding.UTF8.GetString(bSource);
					//string dSource = bSource.ToString();
					string dSources = dSource;//.Split('\r');
					dLF = new DiffList_TextFile(dSources);
				}

			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				MessageBox.Show(ex.Message,"File Error");
				return;
			}
			
			try
			{
				double time = 0;
				DiffEngine de = new DiffEngine();
				time = de.ProcessDiff(sLF,dLF,_level);

				ArrayList rep = de.DiffReport();
				Results dlg = new Results(sLF,dLF,rep,time);
				dlg.ShowDialog();
				dlg.Dispose();
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
					ex.Message,
					Environment.NewLine,
					ex.StackTrace); 
				MessageBox.Show(tmp,"Compare Error");
				return;
			}
			this.Cursor = Cursors.Default;
		}


		private void BinaryDiff(string sFile, string dFile)
		{
			this.Cursor = Cursors.WaitCursor;

			DiffList_BinaryFile sLF = null;
			DiffList_BinaryFile dLF = null;
			try
			{
				sLF = new DiffList_BinaryFile(sFile);
				dLF = new DiffList_BinaryFile(dFile);
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				MessageBox.Show(ex.Message,"File Error");
				return;
			}
			
			try
			{
				double time = 0;
				DiffEngine de = new DiffEngine();
				time = de.ProcessDiff(sLF,dLF,_level);

				ArrayList rep = de.DiffReport();
				
				//BinaryResults dlg = new BinaryResults(rep,time);
				//dlg.ShowDialog();
			//	dlg.Dispose();
								
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
					ex.Message,
					Environment.NewLine,
					ex.StackTrace); 
				MessageBox.Show(tmp,"Compare Error");
				return;
			}
			this.Cursor = Cursors.Default;
		}



		private void cmdCompare_Click(object sender, System.EventArgs e)
		{
			string sFile = cmbSrc.Text.Trim();
			string dFile = cmbDest.Text.Trim();
			/*
			if (!ValidFile(sFile))
			{
				MessageBox.Show("Source file name is invalid.","Invalid File");
				cmbSrc.Focus();
				return;
			}

			if (!ValidFile(dFile))
			{
				MessageBox.Show("Destination file name is invalid.","Invalid File");
				cmbDest.Focus();
				return;
			}
			*/
			if (rbFast.Checked)
			{
				_level = DiffEngineLevel.FastImperfect; 
			}
			else
			{
				if (rbMedium.Checked)
				{
					_level = DiffEngineLevel.Medium;
				}
				else
				{
					_level = DiffEngineLevel.SlowPerfect; 
				}
			}

			if (chkBinary.Checked)
			{
				BinaryDiff(sFile,dFile);
			}
			else
			{
				TextDiff(sFile,dFile);
			}
			
		}

		private void MainForm_Load(object sender, EventArgs e)
		{

		}

		public void setSourceList(string[] srcs)
		{
			this.cmbSrc.Items.Clear();
			foreach (string s in srcs)
				this.cmbSrc.Items.Add(s);
		}
		public void setDestList(string[] dests)
		{
			this.cmbDest.Items.Clear();
			foreach (string s in dests)
				this.cmbDest.Items.Add(s);
		}
	}
}
