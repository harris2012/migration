using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Migration
{
    public class TransConfig : INotifyPropertyChanged
    {
        public TransConfig()
        {
            ReflectionOnlyLoad = true;
        }

        private bool reflectionOnlyLoad;

        /// <summary>
        /// ReflectionOnlyLoad
        /// </summary>
        public bool ReflectionOnlyLoad
        {
            get
            {
                return reflectionOnlyLoad;
            }
            set
            {
                if (reflectionOnlyLoad != value)
                {
                    reflectionOnlyLoad = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ReflectionOnlyLoad)));
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
