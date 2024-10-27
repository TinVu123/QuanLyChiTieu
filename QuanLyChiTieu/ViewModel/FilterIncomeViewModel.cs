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
    public class FilterIncomeViewModel: BaseViewModel
    {
        
            public ICommand FilterCommand { get; set; }
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

            private DateTime _startDate;
            public DateTime StartDate
            {
                get => _startDate;
                set
                {
                    _startDate = value; OnPropertyChanged();
                }
            }

            private DateTime _endDate;
            public DateTime EndDate
            {
                get => _endDate;
                set
                {
                    _endDate = value; OnPropertyChanged();
                }
            }



            public FilterIncomeViewModel()
            {
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
                Category();
                FilterCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { LoadFilter(); });
            }
            public void LoadFilter()
            {
                IncomeList = new ObservableCollection<DisplayListView>();

                using (var db = new QuanLyChiTieuEntities())
                {
                    if (SCategoryIncomeItem != null)
                    {
                        var data = from k in db.ThuNhaps
                                   where UserService.Instance.UserID == k.UserID &&
                                         k.ThoiGian >= StartDate && k.ThoiGian <= EndDate &&
                                         k.DanhMuc == SCategoryIncomeItem.TenMucThu
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
                            IncomeList.Add(ct);
                        }
                        TotalMoney = sum.ToString();
                    }
                    else
                    {
                        var data = from k in db.ThuNhaps
                                   where UserService.Instance.UserID == k.UserID &&
                                         k.ThoiGian >= StartDate && k.ThoiGian <= EndDate
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
                            IncomeList.Add(ct);
                        }
                        TotalMoney = string.Format("{0:#,###}", sum);
                    }
                }
            }

        public ObservableCollection<DanhMucThu> _categoriesIncome = new ObservableCollection<DanhMucThu>(); // Khởi tạo
        public ObservableCollection<DanhMucThu> CategorysIncome
        {
            get { return _categoriesIncome; }
            set
            {
                if (value != _categoriesIncome)
                {
                    _categoriesIncome = value;
                    OnPropertyChanged(nameof(CategorysIncome)); // Sử dụng nameof để tránh lỗi
                }
            }
        }

        private DanhMucThu _scategoryIncome;

        public DanhMucThu SCategoryIncomeItem
        {
            get { return _scategoryIncome; }
            set { _scategoryIncome = value; }
        }

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



    }
}
