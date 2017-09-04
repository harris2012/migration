using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public enum NodeType
    {
        None = 1,

        Class = 2,

        Enum = 3,

        Property = 4,

        Field = 5,

        Method = 6,

        Namespace = 7,

        Root = 8
    }
}
