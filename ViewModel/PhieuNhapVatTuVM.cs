using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.View;
using System.Collections.ObjectModel;

namespace PageNavigation.ViewModel
{
    class PhieuNhapVatTuVM : Utilities.ViewModelBase
    {
		public PhieuNhapVatTuVM()
		{
			ListDetail = new ObservableCollection<CT_PhieuNhapVatTuVM>();
			GRNDate = DateOnly.FromDateTime(DateTime.Now);
		}
		// grn = goods receive note
		private int _grnID;

		public int GRNID
		{
			get { return _grnID; }
			set { _grnID = value; OnPropertyChanged(); }
		}
		private DateOnly _grnDate;

		public DateOnly GRNDate
		{
			get { return _grnDate; }
			set { _grnDate = value; OnPropertyChanged(); }
		}
		private decimal _grnCost;

		public decimal GRNCost
		{
			get { return _grnCost; }
			set { _grnCost = value; OnPropertyChanged(); }
		}
		public ObservableCollection<CT_PhieuNhapVatTuVM> ListDetail { get; set; }
		public void UpdateTotalCost()
		{
			GRNCost = ListDetail.Sum(item => item.TotalAmount);
		}
		public void AddDetail(int productID, string productName, decimal inputPrice)
		{
			var newItem = new CT_PhieuNhapVatTuVM
			{
				ProductID = productID,
				ProductName = productName,
				InputPrice = inputPrice,
				Quantity = 1,
				GRNID = this.GRNID
			};
			newItem.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(CT_PhieuNhapVatTuVM.TotalAmount))
				{
					UpdateTotalCost();
				}
			};
			ListDetail.Add(newItem);
			UpdateTotalCost();
		}
	}
}
