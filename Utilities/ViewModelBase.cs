using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace PageNavigation.Utilities
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        
    }
    public static class GlobalEvents
    {
        public static event Action OnVatTuChanged;

        public static void RaiseVatTuChanged() => OnVatTuChanged?.Invoke();

        public static event Action OnKhachHangChanged;
        public static void RaiseKhachHangChanged() => OnKhachHangChanged?.Invoke();
    }
}
