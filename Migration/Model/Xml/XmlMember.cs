using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Migration.Model.Xml
{
    public class XmlMember
    {
        public string Summary { get; set; }

        public XmlMemberParam[] Param { get; set; }

        public string Returns { get; set; }

        public string Name { get; set; }
    }
}
