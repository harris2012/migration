using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public class MethodNode : Node
    {
        /// <summary>
        /// 【C#】方法返回类型名称
        /// </summary>
        public string ReturnCSharpTypeName { get; set; }

        /// <summary>
        /// 【C#】方法返回类型完整名称
        /// </summary>
        public string ReturnCSharpTypeFullName { get; set; }

        /// <summary>
        /// 【Java】方法返回类型名称
        /// </summary>
        public string ReturnJavaTypeName { get; set; }

        /// <summary>
        /// 【Java】方法返回类型完整名称
        /// </summary>
        public string ReturnJavaTypeFullName { get; set; }

        /// <summary>
        /// 方法的参数
        /// </summary>
        public List<MethodParameter> Parameters { get; set; }

        /// <summary>
        /// 是否是静态方法
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 是属性的get方法
        /// </summary>
        public bool IsGet { get; set; }

        /// <summary>
        /// 是属性的set方法
        /// </summary>
        public bool IsSet { get; set; }

        /// <summary>
        /// 属性名称
        /// example: Name
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性名称
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
                return NodeType.Method;
            }
        }
        #endregion
    }
}
