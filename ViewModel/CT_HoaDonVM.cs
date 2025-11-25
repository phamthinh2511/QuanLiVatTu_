using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    public class CT_HoaDonVM : Utilities.ViewModelBase
    {
		private int _billID;

		public int BillID
		{
			get { return _billID; }
			set { _billID = value; OnPropertyChanged(); }
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
		private int _maxStock;

		public int MaxStock
		{
			get { return _maxStock; }
			set { _maxStock = value; OnPropertyChanged(); }
		}


		private int _quantity;
					
		public int Quantity
		{
			get { return _quantity; }
			set 
			{ 
				if (value > MaxStock)
				{
					// Tự mặc định giá trị về số tồn kho hiện tại
					_quantity = MaxStock;
				}
				else
				{
                    _quantity = value;
                }
				OnPropertyChanged(); 
				OnPropertyChanged(nameof(TotalAmount)); 
			}
        }
		private int _countingUnitID;

		public int CountingUnitID
		{
			get { return _countingUnitID; }
			set { _countingUnitID = value; OnPropertyChanged(); }
		}
		private decimal _outputPrice;

		public decimal OutputPrice
		{
			get { return _outputPrice; }
			set { _outputPrice = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalAmount)); }
        }
		private decimal _totalAmount;

		public decimal TotalAmount
		{
			get { return Quantity * OutputPrice; }
		}

	}
}
