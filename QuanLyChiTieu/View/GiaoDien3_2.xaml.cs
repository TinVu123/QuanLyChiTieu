using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChiThuApp
{
    public partial class GiaoDien3_2 : Window
    {
        // Danh sách ObservableCollection để chứa dữ liệu
        private ObservableCollection<ChiThu> chiThuList;
        private int soThuTu = 0; // Để tăng số thứ tự (STT)

        public GiaoDien3_1()
        {
            InitializeComponent();
            // Khởi tạo danh sách và gán nó làm nguồn dữ liệu cho DataGrid
            chiThuList = new ObservableCollection<ChiThu>();
            dataGridChiThu.ItemsSource = chiThuList;
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Lưu"
        private void Luu_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu dữ liệu hợp lệ
            if (string.IsNullOrEmpty(txtTenCT.Text) || string.IsNullOrEmpty(txtSoTien.Text) || dateThoiGian.SelectedDate == null || string.IsNullOrEmpty(txtChiTiet.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Tăng số thứ tự
            soThuTu++;

            // Thêm dữ liệu vào danh sách chi thu
            chiThuList.Add(new ChiThu
            {
                STT = soThuTu,
                TenCT = txtTenCT.Text,
                SoTien = decimal.Parse(txtSoTien.Text),
                ThoiGian = dateThoiGian.SelectedDate.Value.ToString("dd/MM/yyyy"),
                ChiTiet = txtChiTiet.Text
            });

            // Xóa các trường nhập liệu sau khi thêm xong
            txtTenCT.Clear();
            txtSoTien.Clear();
            dateThoiGian.SelectedDate = null;
            txtChiTiet.Clear();
        }
    }

    // Lớp đại diện cho dữ liệu chi thu
    public class ChiThu
    {
        public int STT { get; set; }
        public string TenCT { get; set; }
        public decimal SoTien { get; set; }
        public string ThoiGian { get; set; }
        public string ChiTiet { get; set; }
    }
}
