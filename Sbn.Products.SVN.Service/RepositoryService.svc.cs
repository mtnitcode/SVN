using Sbn.Products.SVN.DAL;
using Sbn.Products.SVN.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Sbn.Products.SVN.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RepositoryService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RepositoryService.svc or RepositoryService.svc.cs at the Solution Explorer and start debugging.
    public class RepositoryService : IRepositoryService
    {

        public void CheckInBranchContent(DTO.DataFile dataFile)
        {
            throw new NotImplementedException();
        }

        public void CheckOutBranchContent(DTO.DataFile dataFile)
        {
            throw new NotImplementedException();
        }

        public BranchFile UploadBranchContent(DTO.BranchFile dataFile)
        {

            AdoHandler oHandler = new AdoHandler(@System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());

            DataFileDA da = new DataFileDA(oHandler);

            da.SaveBranchContent(dataFile);

            return null;
        }

    }
}
