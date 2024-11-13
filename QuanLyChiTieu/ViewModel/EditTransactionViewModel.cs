using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using QuanLyChiTieu.Models;

namespace QuanLyChiTieu.ViewModel
{
    public class EditTransactionViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private Transaction _transaction;
        public Transaction Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                OnPropertyChanged();
            }
        }

        public EditTransactionViewModel(Transaction transaction)
        {
            Transaction = transaction;

            SaveCommand = new RelayCommand<object>(null, SaveTransaction);
            CancelCommand = new RelayCommand<object>(null, Cancel);
        }

        private void SaveTransaction(object parameter)
        {
            MessageBox.Show($"Đã lưu thay đổi cho giao dịch: {Transaction.Description}");

        }

        private void Cancel(object parameter)
        {
            MessageBox.Show("Hủy bỏ chỉnh sửa");
        }
    }
}
