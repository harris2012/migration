using Migration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace Migration.Convertor
{
    [ValueConversion(typeof(NodeType), typeof(Brush))]
    public class NodeTypeBrushConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nodeType = (NodeType)value;

            Brush brush = Brushes.Black;

            switch (nodeType)
            {
                case NodeType.Class:
                    brush = Brushes.DarkBlue;
                    break;
                case NodeType.Enum:
                    brush = Brushes.Green;
                    break;
                case NodeType.Property:
                    brush = Brushes.Brown;
                    break;
                case NodeType.Field:
                    brush = Brushes.RoyalBlue;
                    break;
                case NodeType.Method:
                    brush = Brushes.Indigo;
                    break;
                case NodeType.None:
                default:
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
