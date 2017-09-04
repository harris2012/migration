using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Model
{
    public class MethodParameter
    {
        public string CSharpTypeName { get; set; }

        public string CSharpTypeFullName { get; set; }

        public string JavaTypeName { get; set; }

        public string JavaTypeFullName { get; set; }

        public string Name { get; set; }
    }
}
