using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    class DonViTinhVM : Utilities.ViewModelBase
    {
		private int _countingUnitID;

		public int CountingUnitID
		{
			get { return _countingUnitID; }
			set { _countingUnitID = value; OnPropertyChanged(); }
		}
		private string _countingUnitName;

		public string CountingUnitName
		{
			get { return _countingUnitName; }
			set { _countingUnitName = value; OnPropertyChanged(); }
		}

	}
}
