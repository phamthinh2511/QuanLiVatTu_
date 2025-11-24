using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    class CT_PhieuNhapVatTuVM : Utilities.ViewModelBase
    {
		private int _grnID;

		public int GRNID
		{
			get { return _grnID; }
			set { _grnID = value; OnPropertyChanged(); }
		}
		private int _productID;

		public int ProductID
		{
			get { return _productID; }
			set { _productID = value; OnPropertyChanged(); }
		}
		private string _productName;

		public string ProductName
		{
			get { return _productName; }
			set { _productName = value; OnPropertyChanged(); }
		}
		private int _quantity;

		public int Quantity
		{
			get { return _quantity; }
			set 
			{ 
				_quantity = value;
                OnPropertyChanged();
				OnPropertyChanged(nameof(TotalAmount));
            }
		}
		private decimal _inputPrice;

		public decimal InputPrice
		{
			get { return _inputPrice; }
			set 
			{
				_inputPrice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalAmount));
            }
		}
		private decimal _totalAmount;

		public decimal TotalAmount
		{
			get { return Quantity * InputPrice; }
		}

	}
}
