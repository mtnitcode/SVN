using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sbn.Products.SVN.DTO
{


    //[KnownType(typeof(BranchFile))]
    //[KnownType(typeof(ProjectVersionFile))]

	//test Change


    [Serializable]
    public class DataFile
    {

        long _fileId;

        //[DataMember]
        public long FileId
        {
            get { return _fileId; }
            set { _fileId = value; }
        }

        private byte[] _fileContent;

        //[DataMember]
        public byte[] FileContent
        {
            get { return _fileContent; }
            set { _fileContent = value; }
        }

        private string _name;

        //[DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _FilePath;
        //[DataMember]
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        Developer _developer;

        //[DataMember]
        public Developer Developer
        {
            get { return _developer; }
            set { _developer = value; }
        }
        private DateTime _lastEditionDate;

        //[DataMember]
        public DateTime LastEditionDate
        {
            get { return _lastEditionDate; }
            set { _lastEditionDate = value; }
        }
        /*
        Project _project;

        //[DataMember]
        public Project Project
        {
            get { return _project; }
            set { _project = value; }
        }
        */
    }
}
