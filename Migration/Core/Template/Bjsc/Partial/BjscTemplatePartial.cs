using Migration.Core.Model;
using Migration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Core.Template.Bjsc
{
    partial class BjscTemplate
    {
        //public List<Namespace> NamespaceList { get; set; }

        public Node Root { get; set; }

        public bool WithInclude { get; set; }
    }
}
