using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Core.Model
{
    public class Namespace
    {
        public string Name { get; set; }

        public List<Type> TypeList { get; set; }
    }
}
