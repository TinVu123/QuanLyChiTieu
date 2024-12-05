using QuanLyChiTieu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace QuanLyChiTieu.ViewModel
{
    public class FilterSpendingViewModel : BaseViewModel
    {
        public ICommand FilterCommand { get; set; }

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

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set{ _startDate = value; OnPropertyChanged();}
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set{ _endDate = value; OnPropertyChanged(); }
        }

        // Constructor
        public FilterSpendingViewModel()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Category();
            FilterCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { LoadFilter(); });
        }

        // Lọc ra theo startDate và endDate và danh mục(nếu muốn)
        public void LoadFilter()
        {
            SpendingList = new ObservableCollection<DisplayListView>();

            using (var db = new QuanLyChiTieuEntities())
            {
                if (SCategorySpending != null)
                {
                    var data = from k in db.ChiTieux
                               where UserService.Instance.UserID == k.UserID
                                     && k.ThoiGian >= StartDate
                                     && k.ThoiGian <= EndDate
                                     && k.DanhMuc == SCategorySpending.TenMucChi
                               select k;

                    int i = 0; long sum = 0;
                    foreach (var k in data.ToList())
                    {
                        // Tạo một đối tượng DisplayListView và thêm vào danh sách
                        var ct = new DisplayListView
                        {
                            STT = ++i,
                            Danhmuc = k.DanhMuc,
                            Thoigian = k.ThoiGian.Value,
                            Sotien = k.SoTien,
                            Chitiet = k.ChiTiet
                        };

                        sum += (long)k.SoTien; // Cộng dồn số tiền
                        SpendingList.Add(ct);
                    }

                    TotalMoney = string.Format("{0:#,###} VND", sum);
                }
                else
                {
                    var data = from k in db.ChiTieux
                               where UserService.Instance.UserID == k.UserID
                                     && k.ThoiGian >= StartDate
                                     && k.ThoiGian <= EndDate
                               select k;

                    int i = 0; long sum = 0;
                    foreach (var k in data.ToList())
                    {
                        // Tạo một đối tượng DisplayListView và thêm vào danh sách
                        var ct = new DisplayListView
                        {
                            STT = ++i,
                            Danhmuc = k.DanhMuc,
                            Thoigian = k.ThoiGian.Value,
                            Sotien = k.SoTien,
                            Chitiet = k.ChiTiet
                        };

                        sum += (long)k.SoTien; // Cộng dồn số tiền
                        SpendingList.Add(ct);
                    }

                    // Hiển thị tổng tiền dưới định dạng VND
                    TotalMoney = string.Format("{0:#,###} VND", sum);
                }

            }
        }

        // Khởi tạo ObservableCollection để lưu Danh sách các mục chi từ database
        public ObservableCollection<DanhMucChi> _categorySpendingList = new ObservableCollection<DanhMucChi>(); 
        public ObservableCollection<DanhMucChi> CategorySpendingList
        {
            get { return _categorySpendingList; }
            set
            {
                if (value != _categorySpendingList)
                {
                    _categorySpendingList = value;
                    OnPropertyChanged(nameof(CategorySpendingList)); // Sử dụng nameof để tránh lỗi
                }
            }
        }

        // Lưu Danh mục được chọn từ combobox
        private DanhMucChi _scategorySpending;
        public DanhMucChi SCategorySpending
        {
            get { return _scategorySpending; }
            set { _scategorySpending = value; }
        }

        // Lấy Các Danh mục từ database rồi lưu vào CategorySpendingList
        public void Category()
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
    }
}
