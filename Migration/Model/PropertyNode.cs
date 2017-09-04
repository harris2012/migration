using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public class PropertyNode : Node
    {
        public string JavaTypeName { get; set; }
        public string JavaTypeFullName { get; set; }

        public string CSharpTypeName { get; set; }
        public string CSharpTypeFullName { get; set; }

        public string BjscTypeName { get; set; }
        public string BjscTypeFullName { get; set; }

        /// <summary>
        /// example: nameProperty
        /// </summary>
        public string PropertyJavaName { get; set; }

        /// <summary>
        /// 【XML注释】摘要
        /// </summary>
        public string Summary { get; internal set; }

        #region Node Members
        public override NodeType NodeType
        {
            get
            {
                return NodeType.Property;
            }
        }
        #endregion
    }
}
