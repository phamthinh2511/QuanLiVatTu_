using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Model;

namespace PageNavigation.ViewModel
{
    class QuaTrinhVanChuyenVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        private int _deliverID;

        public int DeliverID
        {
            get { return _deliverID; }
            set { _deliverID = value; OnPropertyChanged(); }
        }

        private string _recentLocation;

        private int _exportDetailID;

        public int ExportDetailID
        {
            get { return _exportDetailID; }
            set { _exportDetailID = value; OnPropertyChanged(); }
        }

        public string RecentLocation
        {
            get { return _recentLocation; }
            set { _recentLocation = value; OnPropertyChanged(); }
        }

        private DateTime _expectedDeliverTime;

        public DateTime ExpectedDeliverTime
        {
            get { return _expectedDeliverTime; }
            set { _expectedDeliverTime = value; OnPropertyChanged(); }
        }

        public QuaTrinhVanChuyenVM()
        {
            _pageModel = new PageModel();
            DateTime time = DateTime.Now;
            _expectedDeliverTime = time;
        }
    }
}
