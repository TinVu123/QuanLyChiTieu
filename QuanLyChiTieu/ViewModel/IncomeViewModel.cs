using QuanLyChiTieu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyChiTieu.ViewModel
{
    public class IncomeViewModel: BaseViewModel
    {
        public bool Isloaded = true;
        public ICommand FilterIncomeCommand { get; set; }

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
                _monthPicker = value;
                OnPropertyChanged();
                LoadSpending();
            }
        }

        public ICommand ThemCommand { get; set; }

        public IncomeViewModel()
        {
            MonthPicker = DateTime.Today;
            Time = DateTime.Today;
            LoadSpending();
            Category();
            FilterIncomeCommand = new RelayCommand<object>((p) => true, p =>
            {
                View.FilterIncomeView filterSpendingView = new View.FilterIncomeView();
                filterSpendingView.ShowDialog();
            });
        }

        public void LoadSpending()
        {
            IncomeList = new ObservableCollection<DisplayListView>();

            using (var db = new QuanLyChiTieuEntities())
            {
                /* var data = from k in db.ThuNhaps
                            where UserService.Instance.UserID == k.UserID &&
                                  k.ThoiGian.Value.Month == MonthPicker.Month &&
                                  k.ThoiGian.Value.Year == MonthPicker.Year
                            select k;*/
                try
                {
                    var data = db.Database.SqlQuery<ThuNhap>(
                        "exec [dbo].[LayThuNhap] @year, @month, @userid",
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
                            Sotien = (int)k.SoTien,
                            Chitiet = k.ChiTiet
                        };

                        // Kiểm tra nếu IncomeList đã được khởi tạo
                        if (IncomeList == null)
                        {
                            IncomeList = new ObservableCollection<DisplayListView>();
                        }

                        IncomeList.Add(ct);  // Thêm vào danh sách
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }


                /* int i = 0, sum = 0;
                 foreach (var k in data.ToList())
                 {
                     DisplayListView ct = new DisplayListView
                     {
                         STT = ++i,
                         Danhmuc = k.DanhMuc,
                         Thoigian = k.ThoiGian.Value,
                         Sotien = (int)k.SoTien,
                         Chitiet = k.ChiTiet
                     };
                     sum += (int)k.SoTien;
                     IncomeList.Add(ct);
                 }
                 TotalMoney = string.Format("{0:#,###}", sum);*/
            }
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

        private DanhMucThu _scategoryIncome;
        public DanhMucThu SCategoryIncomeItem
        {
            get { return _scategoryIncome; }
            set { _scategoryIncome = value; }
        }

        // Lấy các mục chi từ database rồi thêm vào danh sách CategorysIncome
        public void Category()
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
        }

        // Lấy ra giá trị được chọn từ listview
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

        private string _txbMoney;
        public string Money
        {
            get => _txbMoney;
            set { _txbMoney = value; OnPropertyChanged(); }
        }

        private string _category;
        public string Category1
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

        private void HandleSelectedItem()
        {
            if(SIncomeItem != null)
            {
                Category1 = SIncomeItem.Danhmuc.ToString();
                Money = SIncomeItem.Sotien.ToString("#,##");
                Time = SIncomeItem.Thoigian;
                Note = SIncomeItem.Chitiet?.ToString() ?? "";
            }

        }
    }
}
