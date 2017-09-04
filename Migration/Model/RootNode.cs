using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public class RootNode : Node
    {

        #region Node Members
        public override NodeType NodeType
        {
            get
            {
                return NodeType.Root;
            }
        }
        #endregion
    }
}
