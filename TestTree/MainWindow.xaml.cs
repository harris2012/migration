using System;
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

namespace TestTree
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Node root = new Node { Name = "THIS IS A" };

            for (int i = 0; i < 4; i++)
            {
                var iItem = new Node { Name = string.Format("A_{0}", i.ToString()), IsSelected = i % 2 == 0 };

                for (int j = 0; j < 5; j++)
                {
                    var jItem = new Node { Name = string.Format("B_{0}", j.ToString()), IsSelected = j % 2 == 0 };

                    for (int k = 0; k < 3; k++)
                    {
                        var kItem = new Node { Name = string.Format("C_{0}", k.ToString()), IsSelected = k % 2 == 0 };


                        jItem.Items.Add(kItem);
                    }

                    iItem.Items.Add(jItem);
                }

                root.Items.Add(iItem);
            }

            CTree.ItemsSource = root.Items;

            CTree.SelectedItemChanged += CTree_SelectedItemChanged;
        }

        private void CTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var x = e.NewValue;

            var y = e.OldValue;

            var t = 0;
        }
    }
}
