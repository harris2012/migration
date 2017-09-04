using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TestTree
{
    public class Node : INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<Node> items;
        private NodeType nodeType;
        private bool isSelected;

        public Node()
        {
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

        public NodeType NodeType
        {
            get
            {
                return nodeType;
            }
            set
            {
                if (nodeType != value)
                {
                    nodeType = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(NodeType)));
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if (items != null && items.Count > 0)
                    {
                        foreach (var item in items)
                        {
                            item.IsSelected = value;
                        }
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(isSelected)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
