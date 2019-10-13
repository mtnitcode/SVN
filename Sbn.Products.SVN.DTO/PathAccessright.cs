using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sbn.Products.SVN.DTO
{


    [Serializable]
    public class ProjectAccessright
    {

        Project _project;

        //[DataMember]
        public Project ProjectAccess
        {
			get { return _project; }
			set { _project = value; }
        }
        Developer _developer;

        //[DataMember]
        public Developer Developer
        {
            get { return _developer; }
            set { _developer = value; }
        }

        Accessright _accessright;

        //[DataMember]
        public Accessright Accessright
        {
            get { return _accessright; }
            set { _accessright = value; }
        }

    }
}
