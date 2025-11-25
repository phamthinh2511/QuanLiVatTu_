using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PageNavigation.ViewModel;

namespace PageNavigation.View.PopupDetail
{
    /// <summary>
    /// Interaction logic for KhachHangDetail.xaml
    /// </summary>
    public partial class KhachHangDetail : Window
    {
        private KhachHangVM _originalCustomer;
        private KhachHangVM _tempCustomer;
        public KhachHangDetail(KhachHangVM customer)
        {
            InitializeComponent();
            _originalCustomer = customer;
            _tempCustomer = new KhachHangVM
            {
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                CustomerContact = customer.CustomerContact,
                CustomerAddress = customer.CustomerAddress,
                CustomerGender = customer.CustomerGender,
                CustomerBirth = customer.CustomerBirth,
                GenderOptions = customer.GenderOptions
            };
            this.DataContext = _tempCustomer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _originalCustomer.CustomerName = _tempCustomer.CustomerName;
            _originalCustomer.CustomerContact = _tempCustomer.CustomerContact;
            _originalCustomer.CustomerAddress = _tempCustomer.CustomerAddress;
            _originalCustomer.CustomerGender = _tempCustomer.CustomerGender;
            _originalCustomer.CustomerBirth = _tempCustomer.CustomerBirth;
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
