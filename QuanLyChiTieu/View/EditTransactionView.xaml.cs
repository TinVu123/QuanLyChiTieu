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
using QuanLyChiTieu.Models;
using QuanLyChiTieu.ViewModel;


namespace QuanLyChiTieu.View
{
    public partial class EditTransactionView : Window
    {
        public EditTransactionView(Transaction selectedTransaction)
        {
            InitializeComponent();
            this.DataContext = new EditTransactionViewModel(selectedTransaction);
        }
    }
}
