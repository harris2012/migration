using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public class NamespaceNode : Node
    {

        #region Node Members
        public override NodeType NodeType
        {
            get
            {
                return NodeType.Namespace;
            }
        }
        #endregion
    }
}
