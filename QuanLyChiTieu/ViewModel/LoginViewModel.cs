using QuanLyChiTieu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Text.RegularExpressions;

namespace QuanLyChiTieu.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public Boolean IsLogin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand SignupCommand { get; set; }


        public LoginViewModel()
        {
            IsLogin = false;
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            SignupCommand = new RelayCommand<object>((p) => { return true; }, p =>
            {
                View.SignupView signup = new View.SignupView();
                signup.ShowDialog();
            });
        }

        public void Login(Window p)
        {
            using (var db = new QuanLyChiTieuEntities())
            {
                if (string.IsNullOrEmpty(Email))
                {
                    MessageBox.Show("Vui lòng nhập Email.");
                    return;
                }
                else if (!Regex.IsMatch(Email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                {

                    MessageBox.Show("Vui lòng nhập email hợp lệ.");
                    return;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("Vui lòng nhập Password.");
                    return;
                }
                var count = db.Users.Where(x => x.EmailorPhone == Email && x.Password == Password).Count();

                if (count > 0)
                {
                    IsLogin = true;
                    UserService.Instance.UserID = (from k in db.Users
                                                   where k.EmailorPhone == Email && k.Password == Password
                                                   select k.ID).FirstOrDefault();
                    p.Hide();
                }
                else
                {
                    MessageBox.Show("Sai thông tin");
                }
            }

        }
    }

}
