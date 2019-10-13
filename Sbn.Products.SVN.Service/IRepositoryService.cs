using Sbn.Products.SVN.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Sbn.Products.SVN.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRepositoryService" in both code and config file together.
    [ServiceContract]
    public interface IRepositoryService
    {
        [OperationContract]
        void CheckInBranchContent(DataFile dataFile);

        [OperationContract]
        void CheckOutBranchContent(DataFile dataFile);

        [OperationContract]
        BranchFile UploadBranchContent(BranchFile dataFile);

    
    }
}
