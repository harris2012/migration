using Migration.Core.Template.Java;
using Migration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Migration.Core.Template
{
    internal static class TemplateUtility
    {
        //public static string GetItemsText(Node root)
        //{
        //    string returnValue = string.Empty;

        //    StringBuilder builder = new StringBuilder();

        //    if (root != null && root.Items != null && root.Items.Count > 0)
        //    {
        //        foreach (var item in root.Items)
        //        {
        //            if (item.IsChecked)
        //            {
        //                var nodeTemplate = new CSharpTemplate();
        //                nodeTemplate.Root = item;
        //                var content = nodeTemplate.TransformText();
        //                builder.AppendLine().Append(content);
        //            }
        //        }
        //    }

        //    returnValue = builder.ToString();

        //    return returnValue;
        //}

        public static string GetIndent(int indent)
        {
            string returnValue = string.Empty;

            if (indent > 0)
            {
                returnValue = new string(' ', indent * 4);
            }

            return returnValue;
        }
    }
}
