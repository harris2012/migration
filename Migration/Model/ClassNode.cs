using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public class ClassNode : Node
    {
        public bool IsAbstract { get; set; }

        public string BaseTypeName { get; set; }

        public string BaseTypeFullName { get; set; }

        /// <summary>
        /// 【XML注释】摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 【XML注释】方法返回值
        /// </summary>
        public string Returns { get; set; }


        #region Node Members
        public override NodeType NodeType
        {
            get
            {
                return NodeType.Class;
            }
        }
        #endregion
    }
}
