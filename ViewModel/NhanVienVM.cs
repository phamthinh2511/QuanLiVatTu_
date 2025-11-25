using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageNavigation.Model;

namespace PageNavigation.ViewModel
{
    public class NhanVienVM : Utilities.ViewModelBase
    {
		private int _staffID;

		public int StaffID
		{
			get { return _staffID; }
			set { _staffID = value; OnPropertyChanged(); }
		}
		private string _staffName;

		public string StaffName
		{
			get { return _staffName; }
			set { _staffName = value; OnPropertyChanged(); }
		}
		private DateOnly _staffBirth;

		public DateOnly StaffBirth
		{
			get { return _staffBirth; }
			set { _staffBirth = value; OnPropertyChanged(); }
		}
		private string _staffContact;

		public string StaffContact
		{
			get { return _staffContact; }
			set { _staffContact = value; OnPropertyChanged(); }
		}
		private string _staffPosition;

		public string StaffPosition
		{
			get { return _staffPosition; }
			set { _staffPosition = value; OnPropertyChanged(); }
		}
		private string _staffUsername;

		public string StaffUsername
		{
			get { return _staffUsername; }
			set { _staffUsername = value; OnPropertyChanged(); }
		}
		private string _staffPassword;

		public string StaffPassword
		{
			get { return _staffPassword; }
			set { _staffPassword = value; OnPropertyChanged(); }
		}
		private DateOnly _staffStarted;

		public DateOnly StaffStarted
		{
			get { return _staffStarted; }
			set { _staffStarted = value; OnPropertyChanged(); }
		}

	}
}
