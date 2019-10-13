using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sbn.Products.SVN.DTO
{
    [Serializable]
    public class ProjectVersion
    {
        int _id;

        //[DataMember]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        int _VersionNumber;

        //[DataMember]
        public int VersionNumber
        {
            get { return _VersionNumber; }
            set { _VersionNumber = value; }
        }
        private Project _project;
        //[DataMember]
        public Project Project
        {
            get { return _project; }
            set { _project = value; }
        }

		ProjectAccessright _ProjectAccessright;
		public ProjectAccessright ProjectAccessright
		{
			get { return _ProjectAccessright; }
			set { _ProjectAccessright = value; }
		}

    }
}
