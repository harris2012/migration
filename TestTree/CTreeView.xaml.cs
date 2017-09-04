using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestTree
{
    /// <summary>
    /// CTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class CTreeView : UserControl
    {
        public CTreeView()
        {
            InitializeComponent();
        }

        private void cTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            TheTree.SelectedItemChanged += SelectedItemChanged;
        }

        #region Dependency Properties
        public static readonly DependencyProperty TestProperty = DependencyProperty.Register(nameof(Test), typeof(string), typeof(CTreeView), new PropertyMetadata("HAHAHA"));
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(CTreeView));
        public static readonly RoutedEvent SelectedItemChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedItemChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(CTreeView));
        public string Test
        {
            get
            {
                return (string)GetValue(TestProperty);
            }
            set
            {
                SetValue(TestProperty, value);
            }
        }
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        #endregion

        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged;
    }
}
