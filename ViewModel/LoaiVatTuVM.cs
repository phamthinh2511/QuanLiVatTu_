using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageNavigation.ViewModel
{
    public class LoaiVatTuVM
    {
		private int _productTypeID;

		public int ProductTypeID
		{
			get { return _productTypeID; }
			set { _productTypeID = value; }
		}
		private string _productTypeName;

		public string ProductTypeName
		{
			get { return _productTypeName; }
			set { _productTypeName = value; }
		}
		private string _typeDescription;

		public string TypeDescription
		{
			get { return _typeDescription; }
			set { _typeDescription = value; }
		}

	}
}
