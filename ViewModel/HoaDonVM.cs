using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel 
{
    public class HoaDonVM : Utilities.ViewModelBase
    {
		public HoaDonVM()
		{
			ListDetail = new ObservableCollection<CT_HoaDonVM>();
			BillDate = DateOnly.FromDateTime(DateTime.Now);
		}
		
		private int _billID;

		public int BillID
		{
			get { return _billID; }
			set { _billID = value; OnPropertyChanged(); }
		}
		private int _customerID;

		public int CustomerID
		{
			get { return _customerID; }
			set { _customerID = value; OnPropertyChanged(); }
		}
		private int _staffID;

		public int StaffID
		{
			get { return _staffID; }
			set { _staffID = value; OnPropertyChanged(); }
		}
		private DateOnly _billDate;

		public DateOnly BillDate
		{
			get { return _billDate; }
			set { _billDate = value; OnPropertyChanged(); }
		}
		private decimal _totalCost;

		public decimal TotalCost
		{
			get { return _totalCost; }
			set { _totalCost = value; OnPropertyChanged(); }
		}
        public ObservableCollection<CT_HoaDonVM> ListDetail{ get; set; }
		public void UpdateTotalCost()
		{
			TotalCost = ListDetail.Sum(item => item.TotalAmount);
		}
        public void AddDetail(int productID, string productName, int stockQuantity, decimal outputPrice)
        {
			var existingItem = ListDetail.FirstOrDefault(x => x.ProductID == productID);
			if (existingItem != null)
			{
				if (existingItem.Quantity + 1 <= existingItem.MaxStock)
				{
					existingItem.Quantity++;
                }			
			}
			else
			{
				var newItem = new CT_HoaDonVM
				{
					ProductID = productID,
					ProductName = productName,
					OutputPrice = outputPrice,
					MaxStock = stockQuantity,
					Quantity = 1,
					BillID = this.BillID
				};
                newItem.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(CT_HoaDonVM.TotalAmount))
                    {
                        UpdateTotalCost();
                    }
                };
                ListDetail.Add(newItem);
            } 
            UpdateTotalCost();
        }
    }
}
