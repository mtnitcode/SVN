using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sbn.Products.SVN.DTO
{
    //[DataContract]
    [Serializable]
    public class Solution
    {
		int _id;

		//[DataMember]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}
		string _name;

        //[DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        Project[] _projects;

        //[DataMember]
        public Project[] Projects
        {
            get { return _projects; }
            set { _projects = value; }
        }

    }
}
