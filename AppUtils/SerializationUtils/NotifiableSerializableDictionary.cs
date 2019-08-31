using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils.SerializationUtils
{
    public class NotifiableSerializableDictionary<TKey, TValue> : SerializableDictionary<TKey, TValue>, INotifyPropertyChanged
    {
        public new TValue this[TKey key]
        {
            get { return base[key]; }
            set
            {
                base[key] = value;
                NotifyPropertyChanged();
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
