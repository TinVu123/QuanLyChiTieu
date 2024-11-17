using QuanLyChiTieu.Model;
using QuanLyChiTieu.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyChiTieu.ViewModel
{
    public class SpendingViewModel : BaseViewModel
    {
        public ICommand FilterSpendingCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

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
                TotallSpending();
                reload();
            }
        }
      

        // Constructor
        public SpendingViewModel()
        {
            MonthPicker = DateTime.Today;
            Time = DateTime.Today;

            FilterSpendingCommand = new RelayCommand<object>((p) => true, p =>
            {
                View.FilterSpendingView filterSpendingView = new View.FilterSpendingView();
                filterSpendingView.ShowDialog();
            });
          
            AddCommand = new RelayCommand<object>((p) => true,  p =>
            {
                if (Category != null && Money != string.Empty)
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
                            var kt = db.ChiTieux.Count(t =>
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
                                var chiTieu = new ChiTieu()
                                {
                                    ChiTiet = Note,
                                    SoTien = money,
                                    DanhMuc = Category,
                                    ThoiGian = Time,
                                    UserID = UserService.Instance.UserID,
                                };

                                db.ChiTieux.Add(chiTieu);
                                db.SaveChanges();
                                LoadSpending();
                                TotallSpending();
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


            DeleteCommand = new RelayCommand<object>((p) => true, p =>
            {
                if (SSpendingItem != null)
                {
                    var result = MessageBox.Show("Bạn có muốn xoá không?", "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var db = new QuanLyChiTieuEntities())
                            {
                                var chiTieu = new ChiTieu { ID = SSpendingItem.ID };

                                db.ChiTieux.Attach(chiTieu);
                                db.ChiTieux.Remove(chiTieu);

                                db.SaveChanges();
                                LoadSpending();
                                TotallSpending();
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

            UpdateCommand = new RelayCommand<object>((p) => true, p =>
            {
                if (SSpendingItem != null && Category != null && Money != string.Empty)
                {
                    var m = Money.Replace(@",", string.Empty);
                    m = m.Replace(@" VND", string.Empty);

                    try
                    { 
                         
                        using (var db = new QuanLyChiTieuEntities())
                        {
                            var chiTieu = db.ChiTieux.Where(x => x.ID == SSpendingItem.ID).FirstOrDefault();
                        
                            chiTieu.DanhMuc = Category;
                            chiTieu.ThoiGian = Time;
                            chiTieu.SoTien = decimal.Parse(m);
                            chiTieu.ChiTiet = Note;
                            db.SaveChanges();
                            LoadSpending();
                            TotallSpending();
                            reload();
                            MessageBox.Show("Sua Thanh cong");
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
                else if(SSpendingItem == null)
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

        public void reload()
        {
            SSpendingItem = null;
            Category = null;
            Money = string.Empty; 
            Time = DateTime.Today;
            Note = string.Empty;
        }

        // Hàm lấy dữ liệu ra rồi add nó vào IncomeList( được binding với listview ở IncomeView: ItemsSource="{Binding IncomeViewModel.IncomeList}" )
        public void LoadSpending()
        {
            SpendingList = new ObservableCollection<DisplayListView>();

            using (var db = new QuanLyChiTieuEntities())
            {
                try
                {
                    var data =  db.Database.SqlQuery<ChiTieu>(
                        "exec [dbo].[LayChiTieu] @userid, @month, @year",
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

                        if (SpendingList == null)
                        {
                            SpendingList = new ObservableCollection<DisplayListView>();
                        }
                        SpendingList.Add(ct);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            DanhMuc();
        }

        public void TotallSpending()
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
                        TotalMoney = tongChi.HasValue ? string.Format("{0:#,###} VND", tongChi.Value) : "0 VND";
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
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
            try
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
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            

        }

        // Lấy ra giá trị được chọn từ listview binding với listview trong IncomeView: SelectedItem="{Binding IncomeViewModel.SIncomeItem}"

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

        public void HandleSelectedItem()
        {
            if(_sSpendingItem != null)
            {
            Category = SSpendingItem.Danhmuc;
                Money = string.Format("{0:#,###} VND", SSpendingItem.Sotien);
                Time = SSpendingItem.Thoigian;
            Note = SSpendingItem.Chitiet?.ToString() ?? "";
            }
        }
    }
}
