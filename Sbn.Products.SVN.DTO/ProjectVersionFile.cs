using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Sbn.Products.SVN.DTO
{

    //[DataContract]
    [Serializable]
    public enum ContentStatus
    {
        CheckedIN = 1,
        CheckedOut = 2,
    }

    [Serializable]
    public class ProjectVersionFile : DataFile
    {


        private ContentStatus _developmentStatus;

        //[DataMember]
        public ContentStatus DevelopmentStatus
        {
            get { return _developmentStatus; }
            set { _developmentStatus = value; }
        }

    }
}
