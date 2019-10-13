using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using Sbn.Products.SVN.SVNClient.RepositoryService;


namespace Sbn.Products.SVN.SVNClient
{
    /// <summary>
    /// Interaction logic for splash.xaml
    /// </summary>
    public partial class UploadBranchFiles : Window
    {
        Thread loadingThread;
        Storyboard Showboard;
        Storyboard Hideboard;
        private delegate void ShowDelegate(string txt);
        private delegate void HideDelegate();
		//private delegate void UploadProjectDirectoryDelegate();
		//private delegate void GetBranchDelegate();
		//private delegate void GetProjectDelegate();
		ShowDelegate showDelegate;
        HideDelegate hideDelegate;
		//UploadProjectDirectoryDelegate uploadprojectdiectoryDelegate;
		//GetBranchDelegate getBranchDelegate;
		//GetProjectDelegate getProjectDelegate;



		public static string _repositoryDir = "";
		public static string _projectVersion = "";
		public static string _projectName = "";
		public static string _branchName = "";
		public static string _develpoerName = "";
		public static string _getType = "";

		public UploadBranchFiles()
        {
            InitializeComponent();
            showDelegate = new ShowDelegate(this.showText);
            hideDelegate = new HideDelegate(this.hideText);
			//uploadprojectdiectoryDelegate = new UploadProjectDirectoryDelegate(this.UploadRepositoryBranchChanges);
			//getBranchDelegate = new GetBranchDelegate(this.getLatestVersion);
			//getProjectDelegate = new GetProjectDelegate(this.getProjectVersion);
            Showboard = this.Resources["ShowStoryBoard"] as Storyboard;
            Hideboard = this.Resources["HideStoryBoard"] as Storyboard;

        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

			if (_getType == "offlineDiff")
			{
				var diCoounts = Directory.EnumerateDirectories(_repositoryDir, "*", SearchOption.AllDirectories);
				this.progressUpload.Minimum = 0;
				this.progressUpload.Maximum = diCoounts.Count();
				this.progressUpload.Value = 0;
				loadingThread =  new Thread(MakeOfflineBranchChanges);
				loadingThread.Start();
			}


			if (_getType == "upload")
			{
				var diCoounts = Directory.EnumerateDirectories(_repositoryDir, "*", SearchOption.AllDirectories);
				this.progressUpload.Minimum = 0;
				this.progressUpload.Maximum = diCoounts.Count();
				this.progressUpload.Value = 0;
				loadingThread = new Thread(() => UploadRepositoryBranchChanges(this));  //new Thread(UploadRepositoryBranchChanges);
				loadingThread.Start();
			}
			if (_getType == "getproject")
			{

				loadingThread = new Thread(getProjectVersion);
				loadingThread.Start();
			}
			if (_getType == "getbranch")
			{

				loadingThread = new Thread(getLatestVersion);
				loadingThread.Start();
			}

        }


		

        private void UploadRepositoryBranchChanges(object window)
        {


//			WriteRegistry(projectName + ";" + projectVersion, "", this.txtDeveloperName.Text, "", this.txtBranchName.Text);


			//if (ofd.FileName.Contains(branchState))
			{

				Thread.Sleep(1000);

				string[] dirs = Directory.GetDirectories(_repositoryDir);
				foreach (string item in dirs)
				{

					UploadProjectDirectory(_repositoryDir, item , null);

				}
				Thread.Sleep(1000);

			}

            //close the window
            this.Dispatcher.Invoke(DispatcherPriority.Normal,(Action)delegate() { Close(); });
        }
		private void EmptyFolder(DirectoryInfo directoryInfo)
		{
			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				file.Delete();
			}


			foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
			{
				EmptyFolder(subfolder);
			}
			directoryInfo.Delete(true);

		}
		private void MakeOfflineBranchChanges()
		{


			//			WriteRegistry(projectName + ";" + projectVersion, "", this.txtDeveloperName.Text, "", this.txtBranchName.Text);


			//if (ofd.FileName.Contains(branchState))
			{



				Thread.Sleep(1000);

				try
				{
					EmptyFolder(new DirectoryInfo(_repositoryDir + "\\svnOfflineDiff"));

					//Directory.Delete(_repositoryDir + "\\svnOfflineDiff" , true);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}

				string[] dirs = Directory.GetDirectories(_repositoryDir);
				foreach (string item in dirs)
				{
					//var files = Directory.GetFiles(item);

					//for (int i = 0; i < files.Length; i++)
					//{

					//if (sProjectFilesExtention.Exists(f => f == System.IO.Path.GetExtension(files[i])))
					//if (files[i].Contains(".csproj"))
					//{
					TakeOfflineProjectDirectory(_repositoryDir, item, null);

					//break;
					//}
					//}
				}
				Thread.Sleep(1000);

			}
			//close the window
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { Close(); });
		}


		private void TakeOfflineProjectDirectory(string repositoryPath, string LocalPath, TreeViewItem ParentNode)
		{
			try
			{
				this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { progressUpload.Value += 1; });

				this.Dispatcher.Invoke(showDelegate, LocalPath);
				//this.Dispatcher.Invoke(hideDelegate);

				if (UploadTool.IsIgnorePath(LocalPath)) return;

				List<string> lstFiles = new List<string>();
				string[] sFiles = null;
				if (File.Exists(LocalPath + "\\" + Tool.BranchState))
				{
					sFiles = File.ReadAllLines(LocalPath + "\\" + Tool.BranchState);
					lstFiles = sFiles.ToList();
				}
				else
				{
					if (File.Exists(repositoryPath + "\\" + Tool.ProjectState))
					{
						sFiles = File.ReadAllLines(repositoryPath + "\\" + Tool.ProjectState);
						lstFiles = sFiles.ToList();
					}
					
				}
				{
					//var service = ch.CreateChannel();
					var files = Directory.GetFiles(LocalPath);
					foreach (string item in files)
					{
						var ext = System.IO.Path.GetExtension(item).ToLower();
						if (!Tool.ProjectFilesExclude.Exists(f => f == ext))
						{
							Sbn.Products.SVN.SVNClient.RepositoryService.BranchFile bf = new RepositoryService.BranchFile();
							bf.FilePath = item.Replace(repositoryPath, "");
							bf.Branch = new RepositoryService.Branch { Name = UploadBranchFiles._branchName, LocalPath = repositoryPath };
							bf.DevelopmentStatus = RepositoryService.ContentStatus.CheckedOut;
							bf.Developer = new RepositoryService.Developer { Name = UploadBranchFiles._develpoerName, Role = RepositoryService.DeveloperRole.Developer };
							bf.Branch.ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } };
							FileInfo fi = new FileInfo(item);
							bf.LastEditionDate = fi.LastWriteTime;

							if (File.Exists(repositoryPath + "\\" + bf.FilePath))
							{
								if (!Directory.Exists(System.IO.Path.GetDirectoryName(repositoryPath + "\\svnOfflineDiff\\" + bf.FilePath)))
									Directory.CreateDirectory(System.IO.Path.GetDirectoryName(repositoryPath + "\\svnOfflineDiff\\" + bf.FilePath));

							}

							var dir = lstFiles.Find(x => x.Contains(bf.FilePath));
							if (dir != null)
							{
								DateTime filedate = DateTime.Parse(dir.Split(';')[1]);
								DateTime localfileDate = new DateTime(fi.LastWriteTime.Year, fi.LastWriteTime.Month, fi.LastWriteTime.Day, fi.LastWriteTime.Hour, fi.LastWriteTime.Minute, fi.LastWriteTime.Second);
								var datediff = localfileDate - filedate;
								if (datediff.TotalSeconds > 0)
								{
									File.Copy(repositoryPath + "\\" + bf.FilePath, repositoryPath + "\\svnOfflineDiff\\" + bf.FilePath);
								}

							}
							else
							{
								File.Copy(repositoryPath + "\\" + bf.FilePath, repositoryPath + "\\svnOfflineDiff\\" + bf.FilePath);
							}
						}
					}
				}

				if (Directory.GetDirectories(LocalPath).Length == 0)
					return;

				string[] dirs = Directory.GetDirectories(LocalPath);
				foreach (string directory in dirs)
				{
					//string temp = directory.Substring(directory.LastIndexOf('\\') + 1);
					TakeOfflineProjectDirectory(repositoryPath, directory, null);
				}

				//AppendText(this.txtMessages, "Status : All Directory And Included Files Uploaded Sucessfully\n", Brushes.Red);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
		}
		private void showText(string txt)
        {
            txtLoading.Text = txt;
            BeginStoryboard(Showboard);
        }
        private void hideText()
        {
            BeginStoryboard(Hideboard);
        }

		private  void UploadProjectDirectory(string repositoryPath, string LocalPath, TreeViewItem ParentNode)
		{
			try
			{
				this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate() { progressUpload.Value += 1; });

				this.Dispatcher.Invoke(showDelegate, LocalPath);
                //this.Dispatcher.Invoke(hideDelegate);


                List<string> lstFiles = UploadTool.GetLatestStateForBrach(repositoryPath, LocalPath, _branchName, _develpoerName, _projectVersion, _projectName);

                if (lstFiles != null)
                    UploadTool.UploadDirectory(repositoryPath, LocalPath, _branchName, _develpoerName, _projectVersion, _projectName, lstFiles);
                else
                    return;

                /*
				List<string> lstFiles = new List<string>();
				string[] sFiles = null;
				if (File.Exists(LocalPath + "\\" + Tool.BranchState))
				{
					sFiles = File.ReadAllLines(LocalPath + "\\" + Tool.BranchState);
					lstFiles = sFiles.ToList();
				}
				else
				{
					if (File.Exists(repositoryPath + "\\" + Tool.ProjectState))
					{
						sFiles = File.ReadAllLines(repositoryPath + "\\" + Tool.ProjectState);
						lstFiles = sFiles.ToList();
					}
					else
					{
						using (var ch = new RepositoryService.RepositoryServiceSoapClient())
						{
							List<string> filesDownloadStats = new List<string>();
							int pageCounter = 1;
							var bts = ch.GetProjectFileState(new Branch { Name = UploadBranchFiles._branchName, Developer = new Developer { Name = UploadBranchFiles._develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } } }, "", pageCounter);
							while (bts.Length != 0)
							{
								List<string> lines = new List<string>();

								int icounter = 1;
								foreach (BranchFile bf in bts)
								{
									icounter++;

									if (icounter < ((pageCounter * 5000) - 4999))
									{
										continue;
									}
									if (icounter > (pageCounter * 5000)) break;

									filesDownloadStats.Add(bf.FilePath + ";" + bf.LastEditionDate.ToString());
								}

								pageCounter++;
								bts = ch.GetProjectFileState(new Branch { Name = UploadBranchFiles._branchName, Developer = new Developer { Name = UploadBranchFiles._develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } } }, "", pageCounter);

							}
							File.WriteAllLines(repositoryPath + "\\" + Tool.ProjectState, filesDownloadStats.ToArray());
							sFiles = File.ReadAllLines(repositoryPath + "\\" + Tool.ProjectState);
							lstFiles = sFiles.ToList();

						}
					}
				}

				List<string> uploadedFiles = new List<string>();
				File.WriteAllLines(LocalPath + "\\" + Tool.BranchState, uploadedFiles.ToArray());


				using (var ch = new RepositoryService.RepositoryServiceSoapClient())
				{
					//var service = ch.CreateChannel();
					var files = Directory.GetFiles(LocalPath);
					foreach (string item in files)
					{
						var ext = System.IO.Path.GetExtension(item).ToLower();
						if (!Tool.ProjectFilesExclude.Exists(f => f == ext))
						{
							Sbn.Products.SVN.SVNClient.RepositoryService.BranchFile bf = new RepositoryService.BranchFile();

							bf.FileContent = Tool.CompressFile(item);
							//bf.Name = System.IO.Path.GetFileName(LocalPath);
							bf.FilePath = item.Replace(repositoryPath, "");
							bf.Branch = new RepositoryService.Branch { Name = UploadBranchFiles._branchName , LocalPath = repositoryPath };
							bf.DevelopmentStatus = RepositoryService.ContentStatus.CheckedOut;
							bf.Developer = new RepositoryService.Developer { Name = UploadBranchFiles._develpoerName, Role = RepositoryService.DeveloperRole.Developer };
							bf.Branch.ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } };
							FileInfo fi = new FileInfo(item);
							bf.LastEditionDate = fi.LastWriteTime;

							uploadedFiles = new List<string>();
							uploadedFiles.Add(bf.FilePath + ";" + bf.LastEditionDate.ToString());

							var dir = lstFiles.Find(x => x.Contains(bf.FilePath));
							if (dir != null)
							{
								DateTime filedate = DateTime.Parse(dir.Split(';')[1]);
								DateTime localfileDate = new DateTime(fi.LastWriteTime.Year, fi.LastWriteTime.Month, fi.LastWriteTime.Day, fi.LastWriteTime.Hour, fi.LastWriteTime.Minute, fi.LastWriteTime.Second);
								var datediff = localfileDate - filedate;
								if (datediff.TotalSeconds > 0)
								{
									ch.UploadBranchContent(bf);
								}

							}
							else
							{
								ch.UploadBranchContent(bf);
							}
							File.AppendAllLines(LocalPath + "\\" + Tool.BranchState, uploadedFiles.ToArray());

						}
						//UploadFile(item, FTPPath + "/" + RootDirName);
					}
				}
				//File.WriteAllLines(LocalPath + "\\" + projectLastestGetState, uploadedFiles.ToArray());
                */

				if (Directory.GetDirectories(LocalPath).Length == 0)
					return;

				string[] dirs = Directory.GetDirectories(LocalPath);
				foreach (string directory in dirs)
				{
					UploadProjectDirectory(repositoryPath, directory, null);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}
		}


		private void getLatestVersion()
		{
			try
			{
				var repositoryDir = UploadBranchFiles._repositoryDir;

				using (var ch = new RepositoryService.RepositoryServiceSoapClient())
				{

					List<string> filesDownloadStats = new List<string>();
					int pageCounter = 1;

					var bts = ch.GetProjectBranchState(new Branch { Name = UploadBranchFiles._branchName, Developer = new Developer { Name = UploadBranchFiles._develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } } }, "", pageCounter);
					while (bts.Length != 0)
					{
						this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate ()
						{


							var diCoounts = bts.Length;
							this.progressUpload.Minimum = 0;
							this.progressUpload.Maximum = diCoounts;
							progressUpload.Value = (pageCounter * 5000) - 5000;
						});

						List<string> lines = new List<string>();

						int icounter = 1;
						foreach (BranchFile bf in bts)
						{
							if (icounter < ((pageCounter * 5000) - 4999))
							{
								icounter++;
								continue;
							}
							if (icounter > (pageCounter * 5000)) break;
							icounter++;

							this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate ()
							{
								progressUpload.Value += 1;
								this.txtProgress.Text = progressUpload.Value.ToString() + "/" + this.progressUpload.Maximum.ToString();

							});
							this.Dispatcher.Invoke(showDelegate, bf.FilePath);

							FileInfo fi0 = new FileInfo(repositoryDir + "\\" + bf.FilePath);
							var y = DateTime.Now.Year;


							var splitName = bf.FilePath.Split('.');
							if (splitName.Length > 1 && !Tool.ProjectFilesExclude.Exists(f => f == "." + splitName[splitName.Length - 1]))
							{
								var datediff = bf.LastEditionDate - fi0.LastWriteTime;
								if (datediff.TotalSeconds != 0)//!File.Exists(repositoryDir + "\\" + bf.FilePath) || (bf.LastEditionDate > fi0.LastWriteTime))       //&& fi0.LastWriteTime.Year >= y change by ghmhm 961012
								{

									BranchFile getBF = new BranchFile();
									getBF.FilePath = bf.FilePath;
									getBF.Developer = new Developer { Name = UploadBranchFiles._develpoerName };
									getBF.Branch = new Branch { Name = UploadBranchFiles._branchName, Developer = new Developer { Name = UploadBranchFiles._develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } } };
									try
									{
										BranchFile lastFileVersion = ch.DownloadBranchContent(getBF);
										if (lastFileVersion != null)
										{
											var directory = System.IO.Path.GetDirectoryName(repositoryDir + "\\" + bf.FilePath);

											if (Directory.Exists(directory))
											{

											}
											else
											{
												Directory.CreateDirectory(directory);

											}
											Tool.DecompressContentToFile(lastFileVersion.FileContent, repositoryDir + "\\" + bf.FilePath);
										}
									}
									catch (Exception ex)
									{
										MessageBox.Show(ex.Message + "\n" + ex.StackTrace);

									}
								}

							}

							FileInfo fi = new FileInfo(repositoryDir + "\\" + bf.FilePath);

							filesDownloadStats.Add(bf.FilePath + ";" + fi.LastWriteTime.ToString());
						}
						pageCounter++;
						bts = ch.GetProjectBranchState(new Branch { Name = UploadBranchFiles._branchName, Developer = new Developer { Name = UploadBranchFiles._develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } } }, "", pageCounter);

					}
					File.WriteAllLines(repositoryDir + "\\" + Tool.BranchState, filesDownloadStats.ToArray());


				}

				this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate () { Close(); });

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}


		}

		private void getProjectVersion()
		{
			try
			{
				var repositoryDir = UploadBranchFiles._repositoryDir;
				var projectVersion = UploadBranchFiles._projectVersion;
				var projectName = UploadBranchFiles._projectName;

				using (var ch = new RepositoryService.RepositoryServiceSoapClient())
				{
					List<string> filesDownloadStats = new List<string>();
					int pageCounter = 1;
					var bts = ch.GetProjectFileState(new Branch { Name = UploadBranchFiles._branchName, Developer = new Developer { Name = UploadBranchFiles._develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } } }, "", pageCounter);
					while (bts.Length != 0)
					{
						this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate()
						{
							progressUpload.Value = (pageCounter * 5000) - 4999;

							var diCoounts = bts.Length;
							this.progressUpload.Minimum = 0;
							this.progressUpload.Maximum = diCoounts;
						});

						List<string> lines = new List<string>();

						int icounter = 1;
						foreach (BranchFile bf in bts)
						{
							icounter++;

							if (icounter < ((pageCounter * 5000) - 4999))
							{
								continue;
							}
							if (icounter > (pageCounter * 5000)) break;
																		  
							this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate() { progressUpload.Value += 1; });
							this.Dispatcher.Invoke(showDelegate, bf.FilePath);

							FileInfo fi0 = new FileInfo(repositoryDir + "\\" + bf.FilePath);

							//var y = DateTime.Now.Year;
							var splitName = bf.FilePath.Split('.');
							if (splitName.Length > 1 && !Tool.ProjectFilesExclude.Exists(f => f == "." + splitName[splitName.Length - 1]))
							{
								var diff = (bf.LastEditionDate - fi0.LastWriteTime);
								if (!File.Exists(repositoryDir + "\\" + bf.FilePath) || diff.TotalSeconds > 0)
								{

									// check for backup
									if (File.Exists(repositoryDir + "\\" + bf.FilePath))
									{
										if (!Directory.Exists(System.IO.Path.GetDirectoryName(repositoryDir + "\\svnbackup\\" + bf.FilePath)))
											Directory.CreateDirectory(System.IO.Path.GetDirectoryName(repositoryDir + "\\svnbackup\\" + bf.FilePath));

										File.Copy(repositoryDir + "\\" + bf.FilePath, repositoryDir + "\\svnbackup\\" + bf.FilePath + "-" + fi0.LastWriteTime.ToString("yyyy-MM-dd-HH-mm-ss") + ".svnbak");
									}

									BranchFile getBF = new BranchFile();
									getBF.FilePath = bf.FilePath;
									getBF.Developer = new Developer { Name = UploadBranchFiles._develpoerName };
									getBF.Branch = new Branch { Name = "None", ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(projectVersion), Project = new Project { Name = projectName } } };

									try
									{
										BranchFile lastFileVersion = ch.DownloadProjectContent(getBF);

										if (lastFileVersion != null)
										{
											var directory = System.IO.Path.GetDirectoryName(repositoryDir + "\\" + bf.FilePath);

											if (!Directory.Exists(directory))
												Directory.CreateDirectory(directory);

											Tool.DecompressContentToFile(lastFileVersion.FileContent, repositoryDir + "\\" + bf.FilePath);
										}
									}
									catch(Exception ex) {
										MessageBox.Show(ex.Message + "\n" + ex.StackTrace);

									}
								}
							}
							FileInfo fi = new FileInfo(repositoryDir + "\\" + bf.FilePath);
							filesDownloadStats.Add(bf.FilePath + ";" + fi.LastWriteTime.ToString());

						}
						pageCounter++;
						bts = ch.GetProjectFileState(new Branch { Name = UploadBranchFiles._branchName, Developer = new Developer { Name = UploadBranchFiles._develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(UploadBranchFiles._projectVersion), Project = new Project { Name = UploadBranchFiles._projectName } } }, "", pageCounter);

					}
					File.WriteAllLines(repositoryDir + "\\" + Tool.ProjectState, filesDownloadStats.ToArray());

				}

				this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate() { Close(); });
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}

		}


    }
}
