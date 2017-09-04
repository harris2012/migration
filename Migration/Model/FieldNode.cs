using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public class FieldNode : Node
    {
        public string JavaTypeName { get; set; }
        public string JavaTypeFullName { get; set; }

        public string CSharpTypeName { get; set; }
        public string CSharpTypeFullName { get; set; }

        public string Summary { get; set; }

        public string ConstValue { get; set; }

        public override NodeType NodeType
        {
            get
            {
                return NodeType.Field;
            }
        }
    }
}
