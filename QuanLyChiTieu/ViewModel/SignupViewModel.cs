using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using QuanLyChiTieu.Model;
using System.Runtime.CompilerServices;

namespace QuanLyChiTieu.ViewModel
{
    public class SignupViewModel : BaseViewModel
    {
        public Action CloseAction { get; set; }
        public Boolean IsSignup { get; set; }
        public string EmailorSDT { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public ICommand SignupCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand ConfirmPasswordCommand { get; set; }
        public ICommand AddCommand { get; set; }

        public SignupViewModel()
        {
            IsSignup = false;

            SignupCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Signup(p); });
            PasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            ConfirmPasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { ConfirmPassword = p.Password; });
        }
        void Signup(Window p)
        {

            if (string.IsNullOrEmpty(UserName))
            {
                MessageBox.Show("Vui lòng nhập Name.");
                return;
            }
            if (string.IsNullOrEmpty(EmailorSDT))
            {
                MessageBox.Show("Vui lòng nhập Email.");
                return;
            }
            else if (!Regex.IsMatch(EmailorSDT, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("Vui lòng nhập email hợp lệ.");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Vui lòng nhập Password.");
                return;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                MessageBox.Show("Vui lòng nhập ConfirmPassword.");
                return;
            }
            else if (ConfirmPassword != Password)
            {
                MessageBox.Show("Nhập lại ConfirmPassword.");
                return;
            }
            try
            {
                using (var db = new QuanLyChiTieuEntities())
                {
                    var count = (from k in db.Users
                                 where k.EmailorPhone == EmailorSDT
                                 select k
                           ).Count();
                    if (count == 0)
                    {
                        User a = new User();
                        a.Username = UserName;
                        a.EmailorPhone = EmailorSDT;
                        a.Password = Password;
                        db.Users.Add(a);
                        db.SaveChanges();
                        IsSignup = true;
                        CloseAction();

                        MessageBox.Show("Đăng ký thành công!");
                    }
                    else
                    {
                        MessageBox.Show("EmailorSDT đã tồn tại");
                    }
                }
            }catch(Exception ex) { MessageBox.Show(ex.ToString()); }
            

                
            }
        }
    }

