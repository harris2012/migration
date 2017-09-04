using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Migration.Model
{
    public abstract class Node : INotifyPropertyChanged
    {
        private string name;
        private string fullName;
        private ObservableCollection<Node> items;
        private NodeType nodeType;
        private bool isChecked;

        public Node()
        {
            isChecked = true;
            items = new ObservableCollection<Node>();
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                if (fullName != value)
                {
                    fullName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(FullName)));
                }
            }
        }

        public ObservableCollection<Node> Items
        {
            get
            {
                return items;
            }
            set
            {
                if (items != value)
                {
                    items = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
                }
            }
        }

        public abstract NodeType NodeType
        {
            get;
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    if (items != null && items.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.IsChecked = value;
                        }
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(isChecked)));
                    CheckedStatusChanged?.Invoke(this, new RoutedEventArgs());
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event RoutedEventHandler CheckedStatusChanged;
    }
}
