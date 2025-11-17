using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Model;

namespace PageNavigation.ViewModel 
{
    class SettingsVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public bool CaiDat
        {
            get { return _pageModel.LocationStatus; }
            set { _pageModel.LocationStatus = value; OnPropertyChanged(); }
        }

        public SettingsVM()
        {
            _pageModel = new PageModel();
            CaiDat = true;
        }
    }

}
