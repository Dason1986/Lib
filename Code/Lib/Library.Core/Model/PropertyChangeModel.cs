using System.ComponentModel;
using System.Linq;
using System.Text;
using Library.Annotations;


namespace Library
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertyChangeModel : INotifyPropertyChanged
    {

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
