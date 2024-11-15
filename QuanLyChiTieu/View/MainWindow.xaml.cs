using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using QuanLyChiTieu.ViewModel;

namespace QuanLyChiTieu
{
    public class MainViewModel : BaseViewModel
    {
        // ObservableCollection cho danh sách các khoản chi tiêu
        public ObservableCollection<Expense> Expenses { get; set; }
        public ObservableCollection<Expense> FilteredExpenses { get; set; } // Dùng cho tìm kiếm

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    SearchCommand.Execute(null); // Tự động tìm kiếm khi thay đổi văn bản
                }
            }
        }

        public Expense NewExpense { get; set; } = new Expense();

        public ICommand SearchCommand { get; }
        public ICommand AddExpenseCommand { get; }
        public ICommand RemoveExpenseCommand { get; }

        public MainViewModel()
        {
            Expenses = new ObservableCollection<Expense>();
            FilteredExpenses = new ObservableCollection<Expense>(Expenses);

            // Lệnh tìm kiếm
            SearchCommand = new RelayCommand<object>(_ => SearchExpenses(), _ => !string.IsNullOrWhiteSpace(SearchText));

            // Lệnh thêm khoản chi tiêu
            AddExpenseCommand = new RelayCommand<object>(_ => AddExpense(), _ => CanAddExpense());

            // Lệnh xóa khoản chi tiêu
            RemoveExpenseCommand = new RelayCommand<object>(_ => RemoveExpense(), _ => CanRemoveExpense());
        }

        private void SearchExpenses()
        {
            var keyword = SearchText.ToLower();
            FilteredExpenses.Clear();
            foreach (var expense in Expenses.Where(e => e.Name.ToLower().Contains(keyword) || e.Type.ToLower().Contains(keyword)))
            {
                FilteredExpenses.Add(expense);
            }
        }

        private void AddExpense()
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(NewExpense.Name) || string.IsNullOrWhiteSpace(NewExpense.Amount.ToString()) || string.IsNullOrWhiteSpace(NewExpense.Type))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            // Kiểm tra số tiền hợp lệ
            if (NewExpense.Amount <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền hợp lệ (lớn hơn 0).");
                return;
            }

            Expenses.Add(NewExpense);
            UpdateFilteredExpenses();
            ClearNewExpenseForm();
        }

        private bool CanAddExpense() => !string.IsNullOrWhiteSpace(NewExpense.Name) && NewExpense.Amount > 0;

        private void RemoveExpense()
        {
            if (FilteredExpenses.Count > 0)
            {
                var expenseToRemove = FilteredExpenses.First();
                Expenses.Remove(expenseToRemove);
                UpdateFilteredExpenses();
            }
        }

        private bool CanRemoveExpense() => FilteredExpenses.Any();

        private void UpdateFilteredExpenses()
        {
            FilteredExpenses.Clear();
            foreach (var expense in Expenses)
            {
                FilteredExpenses.Add(expense);
            }
        }

        private void ClearNewExpenseForm()
        {
            NewExpense = new Expense(); // Reset form
            OnPropertyChanged(nameof(NewExpense));
        }
    }

    public class Expense
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
