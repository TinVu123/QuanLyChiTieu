using QuanLyChiTieu.Model;
using QuanLyChiTieu.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyChiTieu.ViewModel
{
    public class SpendingViewModel : BaseViewModel
    {
        public ICommand FilterSpendingCommand { get; set; }

        // ObservableCollection lưu giá trị 
        private ObservableCollection<DisplayListView> _spendingList;
        public ObservableCollection<DisplayListView> SpendingList
        {
            get => _spendingList;
            set { _spendingList = value; OnPropertyChanged(); }
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

        // Constructor
        public SpendingViewModel()
        {
            MonthPicker = DateTime.Today;
            Time = DateTime.Today;
            LoadSpending();

            FilterSpendingCommand = new RelayCommand<object>((p) => true, p =>
            {
                View.FilterSpendingView filterSpendingView = new View.FilterSpendingView();
                filterSpendingView.ShowDialog();
            });
        }


        public void LoadSpending()
        {
            SpendingList = new ObservableCollection<DisplayListView>();

            using (var db = new QuanLyChiTieuEntities())
            {
                var data = from k in db.ChiTieux
                           where UserService.Instance.UserID == k.UserID &&
                                 k.ThoiGian.Value.Month == MonthPicker.Month &&
                                 k.ThoiGian.Value.Year == MonthPicker.Year
                           select k;

                int i = 0, sum = 0;
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
                    SpendingList.Add(ct);
                }
                TotalMoney = string.Format("{0:#,###}", sum);
            }
            DanhMuc();
        }

        // Khởi tạo ObservableCollection để lưu trữ các mục chi
        public ObservableCollection<DanhMucChi> _categorySpendingList = new ObservableCollection<DanhMucChi>(); // Khởi tạo
        public ObservableCollection<DanhMucChi> CategorySpendingList
        {
            get { return _categorySpendingList; }
            set
            {
                if (value != _categorySpendingList)
                {
                    _categorySpendingList = value;
                    OnPropertyChanged(nameof(CategorySpendingList)); 
                }
            }
        }

        // Lưu trữ 1 danh mục được chọn trong combobox
        private DanhMucChi _scategorySpending ;
        public DanhMucChi SCategorySpending 
        {
            get { return _scategorySpending; }
            set { _scategorySpending = value; }
        }

        // Lấy các danh mục từ database và thêm vào ObservableCollection
        public void DanhMuc() 
        {
            using (var db = new QuanLyChiTieuEntities())
            {
                var data = from k in db.DanhMucChis
                           select k;
                foreach (var k in data.ToList())
                {
                    DanhMucChi qj = new DanhMucChi()
                    {
                        ID = k.ID,
                        TenMucChi = k.TenMucChi,
                    };
                    CategorySpendingList.Add(qj);
                }
            }

        }

        // Lấy ra các giá trị của hàng được chọn từ listview
        private DisplayListView _sSpendingItem { get; set; }
        public DisplayListView SSpendingItem
        {
            get { return _sSpendingItem; }
            set
            {
                if (_sSpendingItem != value)
                {
                    _sSpendingItem = value;
                    HandleSelectedItem();
                }
            }
        }

    // Các giá trị binding với textbox
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

        public void HandleSelectedItem()
        {
            if(_sSpendingItem != null)
            {
            Category = SSpendingItem.Danhmuc.ToString();
            Money = SSpendingItem.Sotien.ToString("#,##");
            Time = SSpendingItem.Thoigian;
            Note = SSpendingItem.Chitiet?.ToString() ?? "";
            }
        }
    }
}
