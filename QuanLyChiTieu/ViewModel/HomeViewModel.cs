using QuanLyChiTieu.Model;
using System;
using System.Collections.Generic;
using LiveCharts.Wpf;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using LiveCharts;

namespace QuanLyChiTieu.ViewModel
{
    public class HomeViewModel: BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand DetailSpendingCommand { get; set; }
        public ICommand DetailIcomeCommand { get; set; }

        private string totalSpending;
        public string TotalSpending
        {
            get => totalSpending;
            set { totalSpending = value; OnPropertyChanged(); }
        }

        private string _totalIncome;
        public string TotalIncome
        {
            get => _totalIncome;
            set { _totalIncome = value; OnPropertyChanged(); }
        }

        private SeriesCollection _SeriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _SeriesCollection;
            set
            {
                _SeriesCollection = value;
                OnPropertyChanged();
            }
        }
        public string[] ColumnLabels { get; set; }

        public Func<double, string> Formatter { get; set; }

        private DateTime _monthPicker;

        public DateTime MonthPicker { get => _monthPicker; set { _monthPicker = value; OnPropertyChanged(); LoadSpendingAndIncomeAndIncome(); } }


        public HomeViewModel()
        {
            LoadedCommand = new RelayCommand<Window>((p) => { return true; }, p =>
            {
                if (p == null)
                {
                    return;
                }
                p.Hide();
                View.LoginView loginView = new View.LoginView();
                loginView.ShowDialog();
                var loginVM = loginView.DataContext as LoginViewModel;

                if (loginView.DataContext == null)
                {
                    return;
                }
                if (loginVM.IsLogin )
                {
                    p.Show();
                    MonthPicker = DateTime.Today;
                    LoadSpendingAndIncomeAndIncome();
                }
                else
                {
                    p.Close();
                }
            });


            DetailSpendingCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                View.SpendingView chitiet = new View.SpendingView();
                chitiet.ShowDialog();
            });

            DetailIcomeCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                View.IncomeView thu = new View.IncomeView();
                thu.ShowDialog();
            });
        }

        public void LiveChart()
        {
            SeriesCollection = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Thu",
                    Values = new ChartValues<double>(GetDataLiveChartIncome()),
                },
                new ColumnSeries
                {
                    Title = "Chi",
                    Values = new ChartValues<double>(GetDataLiveChartSpending()) 
                }
            };
            ColumnLabels = new[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };

        }

        // Tinh tong so tien thu va chi
        public void LoadSpendingAndIncomeAndIncome()
        {
            using (var db = new QuanLyChiTieuEntities())
            {
                var tongChi = from k in db.ChiTieux
                              where k.ThoiGian.Value.Month == MonthPicker.Month &&
                                     k.ThoiGian.Value.Year == MonthPicker.Year &&
                                     k.UserID == UserService.Instance.UserID
                              group k by k.ThoiGian into g
                              select g.Sum(p => p.SoTien);

                var tongThu = from k in db.ThuNhaps
                              where k.ThoiGian.Value.Month == MonthPicker.Month &&
                                     k.ThoiGian.Value.Year == MonthPicker.Year &&
                                   UserService.Instance.UserID == k.UserID
                              group k by k.ThoiGian into g
                              select g.Sum(p => p.SoTien);

                if (tongThu != null && tongChi != null)
                {
                    TotalIncome = string.Format("{0:#,###}", tongThu.Sum());
                    TotalSpending = string.Format("{0:#,###}", tongChi.Sum());

                }

            }
            LiveChart();
        }

        // Lấy dữ liệu cho biểu đồ thu
        public double[] GetDataLiveChartIncome()
        {
            Dictionary<int, double> monthDiction = new Dictionary<int, double>
            {
                [1] = 0,
                [2] = 0,
                [3] = 0,
                [4] = 0,
                [5] = 0,
                [6] = 0,
                [7] = 0,
                [8] = 0,
                [9] = 0,
                [10] = 0,
                [11] = 0,
                [12] = 0,
            };

            using (var db = new QuanLyChiTieuEntities())
            {
                var tongTienTheo_monthPicker = db.ThuNhaps

                    .Where(k => k.ThoiGian.Value.Year == _monthPicker.Year && UserService.Instance.UserID == k.UserID)
                    .GroupBy(k => new { k.ThoiGian.Value.Month })
                    .Select(g => new
                    {
                        month = g.Key.Month,
                        TongSoTien = g.Sum(p => p.SoTien)
                    })
                    .ToList();
                foreach (var item in tongTienTheo_monthPicker)
                {
                    monthDiction[item.month] = item.TongSoTien ?? 0;
                }

            }
            var valuesThu = monthDiction.Values.ToArray();
            return valuesThu;

        }

        // Lấy dữ liệu cho biểu đồ chi
        public double[] GetDataLiveChartSpending()
        {
            using (var db = new QuanLyChiTieuEntities())
            {
                Dictionary<int, double> monthChiDiction = new Dictionary<int, double>
                {
                    [1] = 0,
                    [2] = 0,
                    [3] = 0,
                    [4] = 0,
                    [5] = 0,
                    [6] = 0,
                    [7] = 0,
                    [8] = 0,
                    [9] = 0,
                    [10] = 0,
                    [11] = 0,
                    [12] = 0,
                };

                var tongTienTheo_monthPicker = db.ChiTieux
                    .Where(k => k.ThoiGian.Value.Year == _monthPicker.Year && UserService.Instance.UserID == k.UserID)
                    .GroupBy(k => new { k.ThoiGian.Value.Month })
                    .Select(g => new
                    {
                        month = g.Key.Month,
                        TongSoTien = g.Sum(p => p.SoTien)
                    })
                    .ToList();
                foreach (var item in tongTienTheo_monthPicker)
                {
                    monthChiDiction[item.month] = item.TongSoTien ?? 0;
                }
                var valuesChi = monthChiDiction.Values.ToArray();
                return valuesChi;
            }
        }

    }
}
