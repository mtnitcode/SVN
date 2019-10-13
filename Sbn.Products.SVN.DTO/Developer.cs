using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sbn.Products.SVN.DTO
{
    //[DataContract]
    [Serializable]
    public enum DeveloperRole
    {
        Amdin = 1,
        Developer = 2,
        Integrator = 3

    }

    //[DataContract]
    [Serializable]
    public class Developer
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
        private string _password;

        //[DataMember]
        public string Password
        {
          get { return _password; }
          set { _password = value; }
        }

        private DeveloperRole _role;
        //[DataMember]
        public DeveloperRole Role
        {
            get { return _role; }
            set { _role = value; }
        }

        Branch[] _branches;
        //[DataMember]
        public Branch[] Branches
        {
            get { return _branches; }
            set { _branches = value; }
        }
    }
}
