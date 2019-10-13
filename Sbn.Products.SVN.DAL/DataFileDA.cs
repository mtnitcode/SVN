using Sbn.Products.SVN.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbn.Products.SVN.DAL
{
    public class DataFileDA
    {

        AdoHandler _handler;

        public DataFileDA(AdoHandler h)
        {
            _handler = h;

        }

        public BranchFile SaveBranchContent(BranchFile bc)
        {

            List<SqlParameter> oParameters = new List<SqlParameter>();
            BranchFile retBpc = new BranchFile();

            try
            {
                /*
                var dtPrj = _handler.ExecuteDataTable(CommandType.Text, "SELECT ID FROM PROJECT WHERE [Name] = '" + bc.Project.Name + "'");
                if (dtPrj.Rows.Count < 1)
                {
                    var sql = @"INSERT INTO PROJECT ( Name) 
                    VALUES ('" + bc.Project.Name + "' )";
                    var aff = _handler.ExecuteNonQuery(CommandType.Text, sql);
                }
                */

				var dtBranch = _handler.ExecuteDataTable(CommandType.Text, "SELECT ID , LocalPath FROM BRANCH WHERE developerid = (Select id from developer where lower(name) = '" + bc.Developer.Name.ToLower() + "')  and [NAME] = '" + bc.Branch.Name + "'");
				if (dtBranch.Rows.Count < 1)
				{
					var sql = @"INSERT INTO Branch ( Name , CreateDate ,[DeveloperID],[ProjectVersionID] , LocalPath) 
                    VALUES ('" + bc.Branch.Name + "' , GETDATE() , (Select id from developer where lower(name) = '" + bc.Developer.Name.ToLower() + "') , (select pv.id from projectversion pv inner join project p on p.id = pv.projectid where p.name = '" + bc.Branch.ProjectVersion.Project.Name + "' and pv.versionnumber = " + bc.Branch.ProjectVersion.VersionNumber.ToString() + ") , N'" + bc.Branch.LocalPath + "')";
					var aff = _handler.ExecuteNonQuery(CommandType.Text, sql);
				}
				else
				{
					var lpath = dtBranch.Rows[0][1].ToString();
					if(lpath != bc.Branch.LocalPath)
					{
						var sql = @"update Branch set LocalPath = N'" + bc.Branch.LocalPath + "' where id = " + dtBranch.Rows[0][0].ToString() ;
						var aff = _handler.ExecuteNonQuery(CommandType.Text, sql);
					}
				}

                var dtFile = _handler.ExecuteDataTable(CommandType.Text, "SELECT ID FROM PROJECTCONTENT WHERE lower([ContentPath]) = '" + bc.FilePath.ToLower() + "'");
                if(dtFile.Rows.Count < 1)
                {
                    var sql = @"INSERT INTO PROJECTCONTENT ( [ContentPath],[CreateDate],[CreatorDeveloperID],[ProjectVersionID]) 
                    VALUES ('" + bc.FilePath + "' , GETDATE() , (Select id from developer where lower(name) = '" + bc.Developer.Name.ToLower() + "'), (select pv.id from projectversion pv inner join project p on p.id = pv.projectid where p.name = '" + bc.Branch.ProjectVersion.Project.Name + "' and pv.versionnumber = " + bc.Branch.ProjectVersion.VersionNumber.ToString() + "))";
                    var aff = _handler.ExecuteNonQuery(CommandType.Text, sql);
                }

				var selectApplyTo = "select max(id) from branchfile where ApplyToProjectVersion = 0 and developerid =  (Select id from developer where lower(name) = '" + bc.Developer.Name.ToLower() + "') and ProjectContentID = (SELECT [ID] FROM [dbo].[ProjectContent] where [ContentPath] = '" + bc.FilePath.ToLower() + "') and branchid = (SELECT [ID] FROM Branch where Name = '" + bc.Branch.Name.ToLower() + "')";
				var dtApplyTo = _handler.ExecuteDataTable(CommandType.Text, selectApplyTo);
				if (dtApplyTo.Rows.Count > 0 && dtApplyTo.Rows[0][0] != null && dtApplyTo.Rows[0][0].ToString() != "")
				{
					var bfID = long.Parse(dtApplyTo.Rows[0][0].ToString());

					var param1 = new SqlParameter("@FileContent", SqlDbType.VarBinary, int.MaxValue);
					param1.Value = bc.FileContent;
					oParameters.Add(param1);

					int affectedCount = _handler.ExecuteNonQuery(CommandType.Text, "UPDATE BranchFile  SET FileContent = @FileContent, EditionDate = '" + bc.LastEditionDate + "' WHERE ID = " + bfID.ToString(), oParameters.ToArray());
				}
				else
				{
					var param1 = new SqlParameter("@FileContent", SqlDbType.VarBinary, int.MaxValue);
					param1.Value = bc.FileContent;
					oParameters.Add(param1);

					int affectedCount = _handler.ExecuteNonQuery(CommandType.Text, "insert into BranchFile ( DeveloperID , FileContent , EditionDate , ProjectContentID , BranchID , ApplyToProjectVersion) " +
					" values ( (Select id from developer where lower(name) = '" + bc.Developer.Name.ToLower() + "') , @FileContent ,'" + bc.LastEditionDate + "' , (SELECT [ID] FROM [dbo].[ProjectContent] where [ContentPath] = '" + bc.FilePath.ToLower() + "') , (SELECT [ID] FROM Branch where Name = '" + bc.Branch.Name.ToLower() + "'),0)", oParameters.ToArray());
				}
            }
            catch (Exception ex)
            {
                //TODO Log Error
                throw ex;
            }
            finally
            {
                oParameters.Clear();
                oParameters = null;
            }

            return retBpc;
        }

		public BranchFile[] GetProjectContentHistory(BranchFile bf)
		{
			List<BranchFile> bfs = new List<BranchFile>();
			var sql = @"select * from (
						SELECT bf.id ,  pc.ContentPath
	                      ,convert (varchar(19) , bf.editiondate , 121) Editiondate
                          ,b.Name BranchName , d.Name developerName
                          FROM [dbo].[BranchFile] bf
                          inner join ProjectContent pc on bf.ProjectContentID = pc.ID
                          inner join Branch b on b.ID = bf.BranchID
                          inner join ProjectVersion pv on pv.ID = b.ProjectVersionID
						  inner join project p on p.id = pv.ProjectID
                          inner join Developer d on d.ID = bf.DeveloperID
						where 
			              p.name = 'pName' 
						  and 
						  pv.versionnumber = pVersion  and lower(pc.ContentPath) = '" + bf.FilePath.ToLower() + "' ) as totalFiles order by id desc ";
			sql = sql.Replace("pVersion", bf.Branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", bf.Branch.ProjectVersion.Project.Name);
			var dt = _handler.ExecuteDataTable(CommandType.Text, sql);

			foreach (DataRow r in dt.Rows)
			{
				BranchFile b = new BranchFile();
				b.ID = long.Parse(r["id"].ToString());
				b.FilePath = r["ContentPath"].ToString();
				b.LastEditionDate = Convert.ToDateTime(r["Editiondate"].ToString());
				b.DevelopmentStatus = ContentStatus.CheckedOut;
				b.Developer = new Developer { Name = r["developerName"].ToString() , Role = DeveloperRole.Developer };
				b.Branch = new Branch { Name = "BranchName" };

				bfs.Add(b);
			}

			return bfs.ToArray();
		}

		public BranchFile[] GetProjectBranchState(Branch branch, string customPath, int pageNumber)
        {
			var sqlCount = @"SELECT count(*)
                          FROM [dbo].[BranchFile] bf
                          inner join ProjectContent pc on bf.ProjectContentID = pc.ID
                          inner join Branch b on b.ID = bf.BranchID
                          inner join ProjectVersion pv on pv.ID = b.ProjectVersionID
						  inner join project p on p.id = pv.ProjectID
                          inner join (select max(id) mfID , DeveloperID , ProjectContentID from BranchFile group by DeveloperID , ProjectContentID) mbf on mbf.mfID = bf.ID and mbf.DeveloperID = bf.DeveloperID
                          inner join Developer d on d.ID = bf.DeveloperID
						where 
                          d.Name = 'devName'
                          and 
                          b.Name = 'bcName'
                          and
			              p.name = 'pName' 
						  and
                          bf.ApplyToProjectVersion = 0
						  and 
						  pv.versionnumber = pVersion";
			sqlCount = sqlCount.Replace("devName", branch.Developer.Name).Replace("bcName", branch.Name).Replace("pVersion", branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", branch.ProjectVersion.Project.Name);
			var dtCount = _handler.ExecuteDataTable(CommandType.Text, sqlCount);
			var iCount = int.Parse(dtCount.Rows[0][0].ToString());




			List<BranchFile> bfs = new List<BranchFile>();
			for (int i = 1; i <= iCount; i++)
			{
				bfs.Add(new BranchFile { DevelopmentStatus= ContentStatus.CheckedOut });
			}
			var sql = @"SELECT pc.ContentPath
	                          ,convert (varchar(19) , bf.editiondate , 121) Editiondate
                              ,b.Name BranchName
                          FROM [dbo].[BranchFile] bf
                          inner join ProjectContent pc on bf.ProjectContentID = pc.ID
                          inner join Branch b on b.ID = bf.BranchID
                          inner join ProjectVersion pv on pv.ID = b.ProjectVersionID
						  inner join project p on p.id = pv.ProjectID
                          inner join (select max(id) mfID , DeveloperID , ProjectContentID from BranchFile group by DeveloperID , ProjectContentID) mbf on mbf.mfID = bf.ID and mbf.DeveloperID = bf.DeveloperID
                          inner join Developer d on d.ID = bf.DeveloperID
                          where 
                          d.Name = 'devName'
                          and 
                          b.Name = 'bcName'
                          and
			              p.name = 'pName' 
						  and
                          bf.ApplyToProjectVersion = 0
						  and 
						  pv.versionnumber = pVersion  ##customPath## order by pc.ContentPath";
			sql = @"select * from (
						SELECT ROW_NUMBER() OVER ( ORDER BY pc.id ) AS RowNum , pc.id ,  pc.ContentPath
	                      ,convert (varchar(19) , bf.editiondate , 121) Editiondate
                          ,b.Name BranchName
                          FROM [dbo].[BranchFile] bf
                          inner join ProjectContent pc on bf.ProjectContentID = pc.ID
                          inner join Branch b on b.ID = bf.BranchID
                          inner join ProjectVersion pv on pv.ID = b.ProjectVersionID
						  inner join project p on p.id = pv.ProjectID
                          inner join Developer d on d.ID = bf.DeveloperID
						where 
                          d.Name = 'devName'
                          and 
                          b.Name = 'bcName'
                          and
			              p.name = 'pName' 
						  and
                          bf.ApplyToProjectVersion = 0
						  and 
						  pv.versionnumber = pVersion  ##customPath## ) as totalFiles 
						  where
						  RowNum between " +  ((pageNumber*5000)-4999).ToString() + " and " + pageNumber*5000 + "";
			//                          inner join (select max(id) mfID , DeveloperID , ProjectContentID from BranchFile group by DeveloperID , ProjectContentID) mbf on mbf.mfID = bf.ID and mbf.DeveloperID = bf.DeveloperID

//                          union 
//	                      select pc.ContentPath
//		                        ,convert (varchar(19) , pvf.editiondate , 121) Editiondate
//		                        ,'' BranchName
//		                        from ProjectVersionFile pvf
//		                        inner join ProjectContent pc on pvf.ProjectContentID = pc.ID
//								inner join projectversion pv on pv.id = pvf.ProjectVersionID
//								inner join project p on p.id = pv.ProjectID
//		                        where p.name = 'pName' and pvf.ProjectContentID 
//		                        not in (select bf.ProjectContentID from BranchFile bf inner join Developer d on d.ID = bf.DeveloperID where d.Name = 'devName' and ApplyToProjectVersion = 0)
//								and 
//								pv.versionnumber = pVersion ##customPath##";
			if(customPath != "")
			{
				sql = sql.Replace("##customPath##", "and lower(pc.ContentPath) like '" + customPath.ToLower() + "%'");
			}
			else
			{
				sql = sql.Replace("##customPath##", "");

			}
			sql = sql.Replace("devName", branch.Developer.Name).Replace("bcName", branch.Name).Replace("pVersion", branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", branch.ProjectVersion.Project.Name);
            var dt = _handler.ExecuteDataTable(CommandType.Text, sql);

			int iRow = (pageNumber * 5000) - 4999;
			foreach (DataRow r in dt.Rows)
			{
				BranchFile bf = new BranchFile();
				bf.FilePath = r["ContentPath"].ToString();
				bf.LastEditionDate = Convert.ToDateTime(r["Editiondate"].ToString());
				bf.DevelopmentStatus = ContentStatus.CheckedOut;
				bfs[iRow - 1] = bf;
				iRow++;
			}
			if (dt.Rows.Count == 0) bfs = new List<BranchFile>();

            return bfs.ToArray();
        }

		public BranchFile[] GetProjectFileState(Branch branch, string customPath, int pageNumber)
		{

			var sqlCount = @"SELECT count(*) from ProjectVersionFile pvf
		                        inner join ProjectContent pc on pvf.ProjectContentID = pc.ID
								inner join projectversion pv on pv.id = pvf.ProjectVersionID
								inner join project p on p.id = pv.ProjectID
		                        where p.name = 'pName' and pvf.ProjectContentID 
		                        not in (select bf.ProjectContentID from BranchFile bf inner join Developer d on d.ID = bf.DeveloperID where d.Name = 'devName' and ApplyToProjectVersion = 0)
								and 
								pv.versionnumber = pVersion";
			sqlCount = sqlCount.Replace("devName", branch.Developer.Name).Replace("bcName", branch.Name).Replace("pVersion", branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", branch.ProjectVersion.Project.Name);
			var dtCount = _handler.ExecuteDataTable(CommandType.Text, sqlCount);
			var iCount = int.Parse(dtCount.Rows[0][0].ToString());


				

			List<BranchFile> bfs = new List<BranchFile>();
			//for (int i = 1; i <= iCount; i++)
			//{
			//	bfs.Add(new BranchFile {  DevelopmentStatus= ContentStatus.CheckedOut});
			//}


			var sql = @"select * from (
						SELECT ROW_NUMBER() OVER ( ORDER BY pc.id ) AS RowNum , pc.id , pc.ContentPath
		                        ,convert (varchar(19) , pvf.editiondate , 121) Editiondate
		                        ,'' BranchName
		                        from ProjectVersionFile pvf
		                        inner join ProjectContent pc on pvf.ProjectContentID = pc.ID
								inner join projectversion pv on pv.id = pvf.ProjectVersionID
								inner join project p on p.id = pv.ProjectID
		                        where p.name = 'pName' 
								and pv.versionnumber = pVersion ##customPath## ) as totalFiles 
						  where
						  RowNum between " + ((pageNumber * 5000) - 4999).ToString() + " and " + pageNumber * 5000 + "";
			// and pvf.ProjectContentID  not in (select bf.ProjectContentID from BranchFile bf inner join Developer d on d.ID = bf.DeveloperID where d.Name = 'devName' and ApplyToProjectVersion = 0)

			if (customPath != "")
			{
				sql = sql.Replace("##customPath##", "and lower(pc.ContentPath) like '" + customPath.ToLower() + "%'");
			}
			else
			{
				sql = sql.Replace("##customPath##", "");

			}
			sql = sql.Replace("devName", branch.Developer.Name).Replace("bcName", branch.Name).Replace("pVersion", branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", branch.ProjectVersion.Project.Name);
			var dt = _handler.ExecuteDataTable(CommandType.Text, sql);

			int iRow = (pageNumber * 5000) - 4999;
			foreach (DataRow r in dt.Rows)
			{
				BranchFile bf = new BranchFile();
				bf.FilePath = r["ContentPath"].ToString();
				bf.LastEditionDate = Convert.ToDateTime(r["Editiondate"].ToString());
				bf.DevelopmentStatus = ContentStatus.CheckedOut;
                bfs.Add(bf);
                //bfs[iRow-1] = bf;
				iRow++;
			}
			if (dt.Rows.Count == 0) bfs = new List<BranchFile>();

			return bfs.ToArray();
		}

		public BranchFile GetBranchContent(BranchFile dataFile)
		{
			List<SqlParameter> oParameters = new List<SqlParameter>();
			BranchFile retBpc = new BranchFile();

			try
			{

				var param1 = new SqlParameter("@FileContent", SqlDbType.VarBinary, int.MaxValue);
				param1.Direction = ParameterDirection.Output;
				oParameters.Add(param1);
				string sql = @"SELECT @FileContent=bf.filecontent FROM [dbo].[BranchFile] bf
                          inner join ProjectContent pc on bf.ProjectContentID = pc.ID
                          inner join Branch b on b.ID = bf.BranchID
                          inner join ProjectVersion pv on pv.ID= b.ProjectVersionID
                          inner join (select max(id) mfID , DeveloperID , ProjectContentID from BranchFile group by DeveloperID , ProjectContentID) mbf on mbf.mfID = bf.ID and mbf.DeveloperID = bf.DeveloperID
                          inner join Developer d on d.ID = bf.DeveloperID
     					  inner join Project p on p.id = pv.projectid
                          where 
                          d.Name = 'devName'
                          and
                          pv.VersionNumber = pVersion
						  and
						  p.name = 'pName'
                          and 
                          b.Name = 'bcName'
                          and lower(pc.ContentPath) = '" + dataFile.FilePath.ToLower() + "'";
				if(dataFile.ID > 0)
				{
					sql = @"SELECT @FileContent=filecontent FROM [dbo].[BranchFile] where id = " + dataFile.ID.ToString();
				}
				else
					sql = sql.Replace("devName", dataFile.Developer.Name).Replace("bcName", dataFile.Branch.Name).Replace("pVersion", dataFile.Branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", dataFile.Branch.ProjectVersion.Project.Name);

				int affectedCount = _handler.ExecuteNonQuery(CommandType.Text,sql , oParameters.ToArray()) ;
				retBpc.DevelopmentStatus = ContentStatus.CheckedOut;
				retBpc.FileContent =  ((byte[])param1.Value).ToArray();
			}
			catch (Exception ex)
			{
				//TODO Log Error
				throw ex;
			}
			finally
			{
				oParameters.Clear();
				oParameters = null;
			}

			return retBpc;
		}

		public BranchFile GetProjectContent(BranchFile dataFile)
		{
			List<SqlParameter> oParameters = new List<SqlParameter>();
			BranchFile retBpc = new BranchFile();

			try
			{

				var param1 = new SqlParameter("@FileContent", SqlDbType.VarBinary, int.MaxValue);
				param1.Direction = ParameterDirection.Output;
				oParameters.Add(param1);
				string sql = @"SELECT @FileContent=pvf.filecontent 
							  FROM [dbo].[ProjectVersionFile] pvf
							  inner join [dbo].[ProjectContent] pc on pc.id = pvf.[ProjectContentID] 
							  inner join projectVersion pv on pv.id = pvf.projectversionid
							  where pv.versionnumber = pVersion and lower(pc.ContentPath) = '" + dataFile.FilePath.ToLower() + "'";
				sql = sql.Replace("devName", dataFile.Developer.Name).Replace("bcName", dataFile.Branch.Name).Replace("pVersion", dataFile.Branch.ProjectVersion.VersionNumber.ToString());

				int affectedCount = _handler.ExecuteNonQuery(CommandType.Text, sql, oParameters.ToArray());
				retBpc.DevelopmentStatus = ContentStatus.CheckedOut;
				retBpc.FileContent = ((byte[])param1.Value).ToArray();
			}
			catch (Exception ex)
			{
				//TODO Log Error
				throw ex;
			}
			finally
			{
				oParameters.Clear();
				oParameters = null;
			}

			return retBpc;
		}

        public ProjectVersion[] GetProjectsVersion(Developer dev , Solution sln)
        {
            List<ProjectVersion> pvs = new List<ProjectVersion>();
            try
            {

				string sql = @"select p.Name , pv.VersionNumber , pa.accessrighttype from project p 
							inner join ProjectVersion pv on pv.ProjectID = p.id
							inner join projectaccessright pa on pa.projectversionid = pv.id
							inner join developer d on d.id = pa.developerid
							where d.name = 'devName' and p.solutionid = " + sln.Id.ToString();
				sql = sql.Replace("devName", dev.Name);
                var dt = _handler.ExecuteDataTable(CommandType.Text, sql);

                foreach (DataRow r in dt.Rows)
                {

                    ProjectVersion pv = new ProjectVersion();
                    pv.VersionNumber = int.Parse(r["VersionNumber"].ToString());
                    pv.Project = new Project { Name = r["Name"].ToString() };
					pv.ProjectAccessright =  new ProjectAccessright {  Accessright = (Accessright)long.Parse(r["accessrighttype"].ToString()) };
					
                    pvs.Add(pv);
                }
            }
            catch (Exception ex)
            {
                //TODO Log Error
                throw ex;
            }
            finally
            {
            }

            return pvs.ToArray();
        }

		public Solution[] GetSolutions(Developer dev)
		{
			List<Solution> pvs = new List<Solution>();
			try
			{

				string sql = @"select distinct s.Name , s.id from solution s 
							inner join project p on p.solutionid = s.id
							inner join ProjectVersion pv on pv.ProjectID = p.id
							inner join projectaccessright pa on pa.projectversionid = pv.id
							inner join developer d on d.id = pa.developerid
							where d.name = 'devName'";
				sql = sql.Replace("devName", dev.Name);
				var dt = _handler.ExecuteDataTable(CommandType.Text, sql);

				foreach (DataRow r in dt.Rows)
				{
					Solution pv = new Solution();
					pv.Id = int.Parse(r["id"].ToString());
					pv.Name = r["Name"].ToString();
					pvs.Add(pv);
				}
			}
			catch (Exception ex)
			{
				//TODO Log Error
				throw ex;
			}
			finally
			{
			}

			return pvs.ToArray();
		}

		public Branch[] GetDevelopersBranch(Branch branch)
		{
			List<Branch> pvs = new List<Branch>();
			try
			{

				string sql = @"select b.Name , b.LocalPath from project p 
								inner join ProjectVersion pv on pv.ProjectID = p.id
								inner join Branch b on b.projectversionid = pv.id
								inner join Developer d on d.id = b.developerid
								where p.Name = 'pName'
								and pv.versionnumber = pVersion
								and d.name = 'devName'";
				sql = sql.Replace("pVersion", branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", branch.ProjectVersion.Project.Name).Replace("devName", branch.Developer.Name);

				var dt = _handler.ExecuteDataTable(CommandType.Text, sql);

				foreach (DataRow r in dt.Rows)
				{
					Branch b = new Branch();
					b.Name = r["Name"].ToString();
					b.LocalPath = r["LocalPath"].ToString();
					pvs.Add(b);
				}
			}
			catch (Exception ex)
			{
				//TODO Log Error
				throw ex;
			}
			finally
			{
			}

			return pvs.ToArray();
		}

		public DateTime GetServerDate()
		{
			var sql = "select getutcdate()";
			var dt = _handler.ExecuteDataTable(CommandType.Text, sql);
			DateTime curDate = DateTime.Now;
			curDate = (DateTime)dt.Rows[0][0];
			return curDate;
		}


		public void ApplyBranchToProjectVersion(BranchFile branchFile)
		{

			var sql = @"insert into [dbo].[ProjectVersionFile] ([FileContent],[ProjectContentID],[ProjectVersionID],[editiondate])
				SELECT bf.filecontent , bf.ProjectContentID , pv.ID ,bf.editiondate 
				FROM [dbo].[BranchFile] bf
				inner join ProjectContent pc on bf.ProjectContentID = pc.ID
				inner join Branch b on b.ID = bf.BranchID
				inner join ProjectVersion pv on pv.ID= b.ProjectVersionID
				inner join project p on p.id = pv.projectid
				inner join (select max(id) mfID , DeveloperID , ProjectContentID from BranchFile group by DeveloperID , ProjectContentID) mbf on mbf.mfID = bf.ID and mbf.DeveloperID = bf.DeveloperID
				inner join Developer d on d.ID = bf.DeveloperID
				where 
				d.Name = 'devName'
				and
				pv.versionnumber = pVersion 
				and 
				b.Name = 'bcName'
				and
				p.name = 'pName'
				and
				bf.ApplyToProjectVersion = 0
				and bf.ProjectContentID not in (select [ProjectContentID] from [ProjectVersionFile])";

			sql = sql.Replace("devName", branchFile.Developer.Name).Replace("bcName", branchFile.Branch.Name).Replace("pVersion", branchFile.Branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", branchFile.Branch.ProjectVersion.Project.Name);
			int affectedCount = _handler.ExecuteNonQuery(CommandType.Text, sql);
			using (SqlConnection conn = new SqlConnection(_handler._connectionString))
			{
				try
				{
					conn.Open();

					_handler._SQLConnection = conn;

					_handler._SqlTransaction = conn.BeginTransaction();

					sql = @"update [dbo].[ProjectVersionFile] set
						[FileContent] = bf.filecontent , editiondate = bf.editiondate
						FROM [dbo].[BranchFile] bf
						inner join ProjectContent pc on bf.ProjectContentID = pc.ID
						inner join Branch b on b.ID = bf.BranchID
						inner join ProjectVersion pv on pv.ID= b.ProjectVersionID
						inner join (select max(id) mfID , DeveloperID , ProjectContentID from BranchFile group by DeveloperID , ProjectContentID) mbf on mbf.mfID = bf.ID and mbf.DeveloperID = bf.DeveloperID
						inner join Developer d on d.ID = bf.DeveloperID
						inner join Project p on p.id = pv.projectid
						inner join [ProjectVersionFile] pvf on pvf.[ProjectContentID] = bf.ProjectContentID
						where 
						d.Name = 'devName'
						and
						pv.versionnumber = pVersion
						and 
						b.Name = 'bcName'
						and
						p.name = 'pName'
						and
						bf.ApplyToProjectVersion = 0";

					sql = sql.Replace("devName", branchFile.Developer.Name).Replace("bcName", branchFile.Branch.Name).Replace("pVersion", branchFile.Branch.ProjectVersion.VersionNumber.ToString()).Replace("pName", branchFile.Branch.ProjectVersion.Project.Name);
					affectedCount = _handler.ExecuteNonQuery(conn, CommandType.Text, sql);

					sql = "update [dbo].[BranchFile] set ApplyToProjectVersion = 1 where branchid = (select id from branch where lower(name) = '" + branchFile.Branch.Name.ToLower() + "') ";
					affectedCount = _handler.ExecuteNonQuery(conn,CommandType.Text, sql);

					_handler._SqlTransaction.Commit();

				}
				catch		  (Exception ex)
				{
					_handler._SqlTransaction.Rollback();
					throw ex;
				}
				finally
				{
					conn.Close();
					conn.Dispose();
				}
			}

			sql = "[dbo].[FreeTransactionLog]";
			affectedCount = _handler.ExecuteNonQuery(CommandType.StoredProcedure, sql);
		}
	}
}
