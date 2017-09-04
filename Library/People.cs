using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// 人
    /// </summary>
    public abstract class People
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年纪
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Test long
        /// </summary>
        public long TL { get; set; }

        /// <summary>
        /// money
        /// </summary>
        public double Money { get; set; }

        /// <summary>
        /// long型字段
        /// </summary>
        public long LongProperty { get; set; }

        /// <summary>
        /// TD
        /// </summary>
        public List<double> TD { get; set; }
    }
}
