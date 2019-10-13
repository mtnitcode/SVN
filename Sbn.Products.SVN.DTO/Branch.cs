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
    
    public class Branch
    {

		private long _ID;

		//[DataMember]
		public long ID
		{
			get { return _ID; }
			set { _ID = value; }
		}


		private ProjectVersion _projectVersion;


		//[DataMember]
		public ProjectVersion ProjectVersion
		{
			get { return _projectVersion; }
			set { _projectVersion = value; }
		}

		private string _name;
		//[DataMember]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _localPath;
		//[DataMember]
		public string LocalPath
		{
			get { return _localPath; }
			set { _localPath = value; }
		}

		Developer _developer;

        //[DataMember]
        public Developer Developer
        {
            get { return _developer; }
            set { _developer = value; }
        }
    }
}
