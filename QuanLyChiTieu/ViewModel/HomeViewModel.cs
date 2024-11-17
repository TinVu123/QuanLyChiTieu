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
using System.Data.SqlClient;
using System.Data.Entity;

namespace QuanLyChiTieu.ViewModel
{
    public class HomeViewModel: BaseViewModel
    {
        public ICommand LoadedCommand { get; set; }
        public ICommand DetailSpendingCommand { get; set; }
        public ICommand DetailIcomeCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

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

        public int checkYear = 0;

        private DateTime _monthPicker;
        public DateTime MonthPicker
        {
            get => _monthPicker;
            set
            {
                _monthPicker = value;
                OnPropertyChanged();
                LoadSpendingAndIncomeAndIncome();
                if (MonthPicker.Year != checkYear)
                {
                    LiveChart();
                    checkYear = MonthPicker.Year;
                }
            }
        }
      
        public  HomeViewModel()
        {

            LoadedCommand = new RelayCommand<Window>((p) => { return true; }, async p =>
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
                 var thu = new View.IncomeView();
                thu.ShowDialog();
            });

            ReloadCommand = new RelayCommand<object>((p) => { return true; }, p =>
            { 
                    MonthPicker = DateTime.Today;
                    LiveChart();
            });

            }

        public void LiveChart()
        {
            SeriesCollection = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Thu",
                    Values = new ChartValues<decimal>(GetDataLiveChartIncome()),
                },
                new ColumnSeries
                {
                    Title = "Chi",
                    Values = new ChartValues<decimal>(GetDataLiveChartSpending()) 
                }
            };
            ColumnLabels = new[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };

        }

        // Tinh tong so tien thu va chi
        public void LoadSpendingAndIncomeAndIncome()
        {
            try
            {
                using (var db = new QuanLyChiTieuEntities())
                {
                    try
                    {
                        var tongChi =  db.Database.SqlQuery<decimal?>(
                            "exec [dbo].[TinhTongTienChi] @month, @year, @userid",
                            new SqlParameter("@month", MonthPicker.Month),
                            new SqlParameter("@year", MonthPicker.Year),
                            new SqlParameter("@userid", UserService.Instance.UserID)
                        ).SingleOrDefault();

                        TotalSpending = tongChi.HasValue ? string.Format("{0:#,###} VND", tongChi.Value) : "0 VND";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    try
                    {
                        var tongThu =  db.Database.SqlQuery<decimal?>(
                        "exec [dbo].[TinhTongTienThu] @month, @year, @userid",
                        new SqlParameter("@month", MonthPicker.Month),
                        new SqlParameter("@year", MonthPicker.Year),
                        new SqlParameter("@userid", UserService.Instance.UserID)
                        ).SingleOrDefault();


                        TotalIncome = tongThu.HasValue ? string.Format("{0:#,###} VND", tongThu.Value) : "0 VND";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        // Lấy dữ liệu cho biểu đồ thu
        public decimal[] GetDataLiveChartIncome()
        {
            Dictionary<int, decimal> monthDiction = new Dictionary<int, decimal>
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

            try
            {
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
                        monthDiction[item.month] = item.TongSoTien;
                    }

                }
            }catch (Exception e) { MessageBox.Show(e.ToString()); }
            
                var valuesThu = monthDiction.Values.ToArray();
                return valuesThu;

        }

        // Lấy dữ liệu cho biểu đồ chi
        public decimal[] GetDataLiveChartSpending()
        {
                Dictionary<int, decimal> monthChiDiction = new Dictionary<int, decimal>
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
            try
            {
                 using (var db = new QuanLyChiTieuEntities())
                {

                    var tongTienTheo_monthPicker =db.ChiTieux
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
                        monthChiDiction[item.month] = item.TongSoTien;
                    }
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
           
                var valuesChi = monthChiDiction.Values.ToArray();
                return valuesChi;
        }

    }
}
