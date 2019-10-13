using Sbn.Products.SVN.SVNClient.RepositoryService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbn.Products.SVN.SVNClient
{
    public class UploadTool
    {
        public static bool IsIgnorePath(string spath)
        {
            foreach (string s in Tool.ProjectPathsExclude)
            {
                if (s.Contains("*"))
                {
                    if (s != "" && spath.ToLower().Contains(s.Replace("*", "")))
                        return true;
                }
                else
                {
                    if (s != "" && spath.ToLower().Contains(s + "\\"))
                        return true;
                }
            }

            if (spath.ToLower().Contains("svnbackup")) return true;
            if (spath.ToLower().Contains("svnofflinediff")) return true;

            return false;
        }

        public static List<string> GetLatestStateForBrach(string repositoryPath, string LocalPath, string _branchName, string _develpoerName, string _projectVersion, string _projectName)
        {
            if (IsIgnorePath(LocalPath)) return null;

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
                        var bts = ch.GetProjectFileState(new Branch { Name = _branchName, Developer = new Developer { Name = _develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(_projectVersion), Project = new Project { Name = _projectName } } }, "", pageCounter);
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
                            bts = ch.GetProjectFileState(new Branch { Name = _branchName, Developer = new Developer { Name = _develpoerName }, ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(_projectVersion), Project = new Project { Name = _projectName } } }, "", pageCounter);

                        }
                        File.WriteAllLines(repositoryPath + "\\" + Tool.ProjectState, filesDownloadStats.ToArray());
                        sFiles = File.ReadAllLines(repositoryPath + "\\" + Tool.ProjectState);
                        lstFiles = sFiles.ToList();

                    }
                }
            }

            return lstFiles;
        }

        public static void UploadDirectory(string repositoryPath, string LocalPath, string _branchName, string _develpoerName, string _projectVersion, string _projectName, List<string> lstFiles)
        {
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
                        bf.Branch = new RepositoryService.Branch { Name = _branchName, LocalPath = repositoryPath };
                        bf.DevelopmentStatus = RepositoryService.ContentStatus.CheckedOut;
                        bf.Developer = new RepositoryService.Developer { Name = _develpoerName, Role = RepositoryService.DeveloperRole.Developer };
                        bf.Branch.ProjectVersion = new ProjectVersion { VersionNumber = int.Parse(_projectVersion), Project = new Project { Name = _projectName } };
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
        }

    }
}
