using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieu.Model
{
    public class UserService
    {
        private static UserService _instance;
        public static UserService Instance => _instance ?? (_instance = new UserService());
        public int UserID { get; set; }
        private UserService() { }

    }
}
