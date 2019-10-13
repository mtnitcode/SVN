using Sbn.Products.SVN.DAL;
using Sbn.Products.SVN.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace Sbn.Products.SVN.SourceRepositoryWebService
{
    /// <summary>
    /// Summary description for RepositoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RepositoryService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [XmlInclude(typeof(DataFile))]
        [XmlInclude(typeof(Project))]
        [XmlInclude(typeof(Developer))]
        [XmlInclude(typeof(ProjectVersion))]
        [XmlInclude(typeof(DataFile))]
        [XmlInclude(typeof(ContentStatus))]
        public BranchFile UploadBranchContent(DTO.BranchFile dataFile)
        {
            AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());

            DataFileDA da = new DataFileDA(oHandler);

            da.SaveBranchContent(dataFile);

            BranchFile bf = new BranchFile();
            bf.DevelopmentStatus = ContentStatus.CheckedOut;
            return bf;
        }

		[WebMethod]
		[XmlInclude(typeof(DataFile))]
		[XmlInclude(typeof(Project))]
		[XmlInclude(typeof(Developer))]
		[XmlInclude(typeof(ProjectVersion))]
		[XmlInclude(typeof(DataFile))]
		[XmlInclude(typeof(ContentStatus))]
		public BranchFile DownloadBranchContent(DTO.BranchFile dataFile)
		{
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());

			DataFileDA da = new DataFileDA(oHandler);

			return da.GetBranchContent(dataFile);
		}
		[WebMethod]
		[XmlInclude(typeof(DataFile))]
		[XmlInclude(typeof(Project))]
		[XmlInclude(typeof(Developer))]
		[XmlInclude(typeof(ProjectVersion))]
		[XmlInclude(typeof(DataFile))]
		[XmlInclude(typeof(ContentStatus))]
		public BranchFile DownloadProjectContent(DTO.BranchFile dataFile)
		{
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());

			DataFileDA da = new DataFileDA(oHandler);

			return da.GetProjectContent(dataFile);
		}


		[WebMethod]
		[XmlInclude(typeof(DataFile))]

		public BranchFile[] GetProjectBranchState(Branch branch, string customPath , int pageNumber)
		{
			/*
			List<BranchFile> bfs = new List<BranchFile>();
			bfs.Add(new BranchFile { DevelopmentStatus = ContentStatus.CheckedIN  });

			return bfs.ToArray();
			*/
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			return da.GetProjectBranchState(branch, customPath, pageNumber);
		}

		[WebMethod]
		[XmlInclude(typeof(DataFile))]

		public BranchFile[] GetProjectContentHistory(BranchFile bf)
		{

			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			return da.GetProjectContentHistory(bf);

		}

		[WebMethod]
		[XmlInclude(typeof(DataFile))]

		public BranchFile[] GetProjectFileState(Branch branch, string customPath , int pageNumber)
		{
			/*
			List<BranchFile> bfs = new List<BranchFile>();
			bfs.Add(new BranchFile { DevelopmentStatus = ContentStatus.CheckedIN  });

			return bfs.ToArray();
			*/
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			return da.GetProjectFileState(branch, customPath, pageNumber);
		}
		[WebMethod]
		[XmlInclude(typeof(DataFile))]
		public Solution[] GetSolutions(Developer developer)
		{
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			return da.GetSolutions(developer);
		}
		[WebMethod]
		[XmlInclude(typeof(DataFile))]
		public ProjectVersion[] GetProjects(Developer developer , Solution sln)
		{
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			return da.GetProjectsVersion(developer , sln);
		}
		[WebMethod]
		[XmlInclude(typeof(DataFile))]
		public Branch[] GetDevelopersBranch(Branch branch)
		{
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			return da.GetDevelopersBranch(branch);
		}

        [WebMethod]
        [XmlInclude(typeof(DataFile))]
		public BranchFile GetProjectBranch(BranchFile branchFile)
		{
			/*
			List<BranchFile> bfs = new List<BranchFile>();
			bfs.Add(new BranchFile { DevelopmentStatus = ContentStatus.CheckedIN  });

			return bfs.ToArray();
			*/
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			return da.GetBranchContent(branchFile);
		}

        [WebMethod]
        [XmlInclude(typeof(DataFile))]
		public void ApplyBranchToProjectVersion(BranchFile branchFile)
		{
			/*
			List<BranchFile> bfs = new List<BranchFile>();
			bfs.Add(new BranchFile { DevelopmentStatus = ContentStatus.CheckedIN  });

			return bfs.ToArray();
			*/
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			da.ApplyBranchToProjectVersion(branchFile);
		}

		[WebMethod]
		[XmlInclude(typeof(DataFile))]
		public DateTime GetServerDatetime()
		{	  /*
			AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
			DataFileDA da = new DataFileDA(oHandler);
			*/
			return DateTime.UtcNow;
		}

    }
}
