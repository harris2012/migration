using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Core.Template.Java
{
    partial class HtmlTemplate
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime GenTime { get; set; }

        /// <summary>
        /// 生成的代码
        /// </summary>
        public string Code { get; set; }
    }
}
