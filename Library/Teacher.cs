using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    /// <summary>
    /// 教师
    /// </summary>
    class Teacher : Employee
    {
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 问候
        /// </summary>
        public void SayHi()
        {

        }

        /// <summary>
        /// 向某人问候
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>ReturnValue</returns>
        public string GetSayHi(string name)
        {
            return string.Format("Hi, {0}", name);
        }
    }
}
