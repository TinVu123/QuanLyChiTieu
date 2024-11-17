using QuanLyChiTieu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyChiTieu.ViewModel
{
    public class IncomeViewModel: BaseViewModel
    {
        public bool Isloaded = true;
        public ICommand FilterIncomeCommand { get; set; }
        public ICommand AddCommand {  get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand RefreshCommand { get; set; }


        // Tất cả thuộc tính sẽ được binding với IncomeView
        private ObservableCollection<DisplayListView> _incomeList;
        public ObservableCollection<DisplayListView> IncomeList
        {
            get => _incomeList;
            set { _incomeList = value; OnPropertyChanged(); }
        }

        private string _totalMoney;
        public string TotalMoney
        {
            get => _totalMoney;
            set { _totalMoney = value; OnPropertyChanged(); }
        }

        private DateTime _monthPicker;
        public DateTime MonthPicker
        {
            get => _monthPicker;
            set
            {
                // Tất cả sẽ load lại khi thay đổi MonthPicker
                _monthPicker = value;
                OnPropertyChanged();
                LoadIncome();
                TotalIncome();
                reload();
            }
        }

        public IncomeViewModel()
        {
            MonthPicker = DateTime.Today;
            CategoryList();
            FilterIncomeCommand = new RelayCommand<object>((p) => true, p =>
            {
                View.FilterIncomeView filterSpendingView = new View.FilterIncomeView();
                filterSpendingView.ShowDialog();
            });

            AddCommand = new RelayCommand<object>((p) => true,  p =>
            {
                if (Category != null && Money != null)
                {
                    try
                    {
                        if (Money.Contains("."))
                        {
                            Money = Money.Replace(@".", string.Empty);
                        }
                        else if (Money.Contains(","))
                        {
                            Money = Money.Replace(@",", string.Empty);
                        }
                        Money = Money.Replace(@" VND", string.Empty);

                        var money = decimal.Parse(Money);
                        using (var db = new QuanLyChiTieuEntities())
                        {
                            var kt = db.ThuNhaps.Count(t =>
                                t.DanhMuc == Category &&
                                t.UserID == UserService.Instance.UserID &&
                                t.SoTien == money && t.ChiTiet == Note &&
                                t.ThoiGian == Time
                            );
                            if (kt > 0)
                            {
                                MessageBox.Show("Thông tin đã tồn tại ");
                            }
                            else
                            {
                                var thuNhap = new ThuNhap()
                                {
                                    ChiTiet = Note,
                                    SoTien = money,
                                    DanhMuc = Category,
                                    ThoiGian = Time,
                                    UserID = UserService.Instance.UserID,
                                };

                                db.ThuNhaps.Add(thuNhap);
                                db.SaveChanges();
                                LoadIncome();
                                TotalIncome();
                                reload();
                                MessageBox.Show("Đã thêm thông tin thành công!");


                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.HResult == -2146233033)
                            MessageBox.Show($"Vui lòng nhập lại thông tin");
                        else
                        {
                            MessageBox.Show($"Lỗi: {e.Message}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền thông tin");
                }
            });

            DeleteCommand = new RelayCommand<object>((p) => true,  p =>
            {
                if(SIncomeItem != null)
                {
                    var result = MessageBox.Show("Bạn có muốn xoá không?", "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var db = new QuanLyChiTieuEntities())
                            {
                                var thuNhap = new ThuNhap { ID = SIncomeItem.ID };

                                db.ThuNhaps.Attach(thuNhap);
                                db.ThuNhaps.Remove(thuNhap);

                                db.SaveChanges();
                                LoadIncome();
                                TotalIncome();
                                reload();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Đã xảy ra lỗi khi xoá: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn thông tin muốn xoá");
                }

            });

            UpdateCommand = new RelayCommand<object>((p) => true,  p =>
            {
                if (SIncomeItem != null && Category != null && Money != null)
                {
                    var m = Money.Replace(@",", string.Empty);
                    m = m.Replace(@" VND", string.Empty);
                    try
                    {

                        using (var db = new QuanLyChiTieuEntities())
                        {
                            var thuNhap = db.ThuNhaps.Where(x => x.ID == SIncomeItem.ID).FirstOrDefault();
                            thuNhap.DanhMuc = Category;
                            thuNhap.ThoiGian = Time;
                            thuNhap.SoTien = decimal.Parse(m);
                            thuNhap.ChiTiet = Note;
                            db.SaveChanges();
                            LoadIncome();
                            reload();
                            TotalIncome();
                            MessageBox.Show("Sửa thành công");
                        }
                    }
                    catch (Exception e)
                    {
                        if(e.HResult == -2146233033 )
                        MessageBox.Show($"Vui lòng nhập lại thông tin");
                        else
                        {
                            MessageBox.Show($"Lỗi: {e.Message}");
                        }
                    }
                }
                else if( SIncomeItem == null )
                {
                    MessageBox.Show("Vui lòng chọn thông tin muốn sửa");
                }
                else
                {
                    MessageBox.Show("Vui lòng không để trống thông tin");
                }
            });
            
            RefreshCommand = new RelayCommand<object>((p) => true, p =>
            {
                reload();
            });
        }

        // Hàm lấy dữ liệu ra rồi add nó vào IncomeList( được binding với listview ở IncomeView: ItemsSource="{Binding IncomeViewModel.IncomeList}" )
        public void LoadIncome()
        {
            IncomeList = new ObservableCollection<DisplayListView>();
            try
            {
                using (var db = new QuanLyChiTieuEntities())
                {
                    var data =  db.Database.SqlQuery<ThuNhap>(
                        "exec [dbo].[LayThuNhap] @userid, @month,  @year",
                        new SqlParameter("@userid", UserService.Instance.UserID),
                        new SqlParameter("@month", MonthPicker.Month),
                        new SqlParameter("@year", MonthPicker.Year)
                    ).ToList();

                    int i = 0;
                    foreach (var k in data)
                    {
                        DisplayListView ct = new DisplayListView
                        {
                            STT = ++i,
                            Danhmuc = k.DanhMuc,
                            Thoigian = (DateTime)k.ThoiGian,
                            Sotien =  k.SoTien,
                            Chitiet = k.ChiTiet,
                            ID = k.ID,
                        };
                        if (IncomeList == null)
                        {
                            IncomeList = new ObservableCollection<DisplayListView>();
                        }

                        IncomeList.Add(ct);  // Thêm vào danh sách
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        public void reload()
        {
            SIncomeItem = null;
            Category = null;
            Money = string.Empty;  // Nếu Money là kiểu decimal, bạn có thể đặt về 0
            Time = DateTime.Today;
            Note = string.Empty;

        }

        // Hàm tính tổng tiền thu
        public void TotalIncome()
        {
            try
            {
                using (var db = new QuanLyChiTieuEntities())
                {
                   
                        var tongThu =  db.Database.SqlQuery<decimal?>(
                            "exec [dbo].[TinhTongTienThu] @month, @year, @userid",
                            new SqlParameter("@month", MonthPicker.Month),
                            new SqlParameter("@year", MonthPicker.Year),
                            new SqlParameter("@userid", UserService.Instance.UserID)
                        ).SingleOrDefault();
                        TotalMoney = tongThu.HasValue ? string.Format("{0:#,###} VND", tongThu.Value) : "0 VND";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        public ObservableCollection<DanhMucThu> _categoriesIncome = new ObservableCollection<DanhMucThu>(); 
        public ObservableCollection<DanhMucThu> CategorysIncome
        {
            get { return _categoriesIncome; }
            set
            {
                if (value != _categoriesIncome)
                {
                    _categoriesIncome = value;
                    OnPropertyChanged(nameof(CategorysIncome)); 
                }
            }
        }

        // Lấy các danh mục chi từ database rồi thêm vào danh sách CategorysIncome
        public void CategoryList()
        {
            try
            {
                using (var db = new QuanLyChiTieuEntities())
                {
                    var data = from k in db.DanhMucThus
                               select k;
                    foreach (var k in data.ToList())
                    {
                        DanhMucThu qj = new DanhMucThu()
                        {
                            ID = k.ID,
                            TenMucThu = k.TenMucThu,
                        };
                        CategorysIncome.Add(qj);
                    }
                }
            }catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        // Lấy ra giá trị được chọn từ listview binding với listview trong IncomeView: SelectedItem="{Binding IncomeViewModel.SIncomeItem}"
        private DisplayListView _sIncomeItem { get ; set; }
        public DisplayListView SIncomeItem
        {
            get { return _sIncomeItem; }
            set
            {
                if (_sIncomeItem != value)
                {
                    _sIncomeItem = value;
                    HandleSelectedItem();
                }
            }
        }

        // Đây là các giá trị binding với textbox

        private string _txbMoney;
        public string Money
        {
            get => _txbMoney;
            set { _txbMoney = value; OnPropertyChanged(); }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set { _note = value; OnPropertyChanged(); }
        }

        // Thực hiện gán giá trị lấy được từ listview khi click vào hàng dữ liệu nào đó
        private void HandleSelectedItem()
        {
            if(SIncomeItem != null)
            {
                Category = SIncomeItem.Danhmuc; //SCategoryIncomeItem
                //Money = SIncomeItem.Sotien;
                Money = string.Format("{0:#,###} VND", SIncomeItem.Sotien);
                Time = SIncomeItem.Thoigian;
                Note = SIncomeItem.Chitiet?.ToString() ?? "";
            }

        }
    }
}
