using System;
using System.Collections;
using System.Collections.Generic;
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

namespace Migration.Controls
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
            TheTree.SelectedItemChanged += (s, x) => { SelectedItemChanged?.Invoke(sender, x); };
        }

        public void OnRefreshCheckedStatus(object refreshNode)
        {
            CheckedStatusChanged?.Invoke(this, new RoutedPropertyChangedEventArgs<object>(null, refreshNode));
        }

        #region Dependency Properties
        public static readonly DependencyProperty TestProperty = DependencyProperty.Register(nameof(Test), typeof(string), typeof(CTreeView), new PropertyMetadata("HAHAHA"));
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(CTreeView));
        //public static readonly RoutedEvent SelectedItemChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedItemChanged), RoutingStrategy.Direct, typeof(RoutedPropertyChangedEventHandler<object>), typeof(CTreeView));
        //public static readonly RoutedEvent CheckedStatusChangedEvent = EventManager.RegisterRoutedEvent(nameof(CheckedStatusChanged), RoutingStrategy.Direct, typeof(RoutedPropertyChangedEventHandler<object>), typeof(CTreeView));
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
        public event RoutedPropertyChangedEventHandler<object> CheckedStatusChanged;

        /// <summary>
        /// 获取当前选中项
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return TheTree.SelectedItem;
            }
        }

        public void SetRootAsCurrent()
        {
            if (TheTree.Items != null && TheTree.Items.Count > 0)
            {
                var item = (TreeViewItem)TheTree.ItemContainerGenerator.ContainerFromIndex(0);

                item.IsExpanded = true;

                item.IsSelected = true;
            }
        }
    }
}
