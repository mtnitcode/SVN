using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Sbn.Products.SVN.DTO
{
    [Serializable]
    public class BranchFile : DataFile
    {
        long _ID;
        //[DataMember]
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        Branch _branch;
        //[DataMember]
        public Branch Branch
        {
            get { return _branch; }
            set { _branch = value; }
        }

        private ContentStatus _developmentStatus;
        //[DataMember]
        public ContentStatus DevelopmentStatus
        {
            get { return _developmentStatus; }
            set { _developmentStatus = value; }
        }
    }
}
