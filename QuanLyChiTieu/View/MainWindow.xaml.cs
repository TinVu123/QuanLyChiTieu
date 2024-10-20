using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

public partial class MainWindow : Window
{
    public ObservableCollection<Expense> Expenses { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        Expenses = new ObservableCollection<Expense>();
        ExpenseList.ItemsSource = Expenses;
    }
}

public class Expense
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}


private void btnAdd_Click(object sender, RoutedEventArgs e)
{
    // Lấy dữ liệu từ các ô nhập liệu
    string name = txtName.Text;
    decimal amount;
    if (!decimal.TryParse(txtAmount.Text, out amount))
    {
        MessageBox.Show("Vui lòng nhập số tiền hợp lệ.");
        return;
    }
    string type = ((ComboBoxItem)cbType.SelectedItem).Content.ToString();
    string description = txtDescription.Text;

    // Tạo đối tượng Expense mới và thêm vào danh sách
    Expenses.Add(new Expense { Name = name, Amount = amount, Type = type, Description = description });

    // Xóa dữ liệu trong các ô nhập liệu sau khi thêm
    txtName.Clear();
    txtAmount.Clear();
    txtDescription.Clear();
    cbType.SelectedIndex = -1;
}


private void btnRemove_Click(object sender, RoutedEventArgs e)
{
    if (ExpenseList.SelectedItem != null)
    {
        Expenses.Remove((Expense)ExpenseList.SelectedItem);
    }
    else
    {
        MessageBox.Show("Vui lòng chọn khoản chi tiêu cần xóa.");
    }
}
