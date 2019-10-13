using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sbn.Products.SVN.DTO
{
    [Serializable]
    public enum Accessright
    {
        Read =1,
        Write = 2
    }

    [Serializable]
    public class PathAccessright
    {

        string _path;

        //[DataMember]
        public string Path
        {
            get { return _path; }
            set { _path = value; }
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
