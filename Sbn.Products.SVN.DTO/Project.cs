using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sbn.Products.SVN.DTO
{
    [Serializable]
    public class Project
    {
        long _id;

        //[DataMember]
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        //[DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private ProjectVersionFile[] _contents;

        //[DataMember]
        public ProjectVersionFile[] Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

    }
}
