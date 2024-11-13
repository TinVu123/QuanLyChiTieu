using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using QuanLyChiTieu.Models;
using System.Windows;
using QuanLyChiTieu.View;


namespace QuanLyChiTieu.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand LogoutCommand { get; }
        public ICommand EditCommand { get; }

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel()
        {
            LogoutCommand = new RelayCommand<object>(null, Logout);
            EditCommand = new RelayCommand<object>(CanEditTransaction, EditTransaction);
        }

        private void Logout(object parameter)
        {
            MessageBox.Show("Đăng xuất thành công!");
        }

        private void EditTransaction(object parameter)
        {
            if (SelectedTransaction != null)
            {
                EditTransactionView editView = new EditTransactionView(SelectedTransaction);
                editView.ShowDialog();
            }
        }

        private bool CanEditTransaction(object parameter)
        {
            return SelectedTransaction != null;
        }
    }
}
