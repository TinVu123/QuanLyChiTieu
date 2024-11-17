using QuanLyChiTieu.Model;
using QuanLyChiTieu.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieu.ViewModel
{
    // ViewModel chính chứa các viewmodel con
    public class MainViewModel
    {
        private DatePickerCalendar datePickerCalendar = new DatePickerCalendar();
        public DatePickerCalendar DatePickerCalendar
        {
            get { return datePickerCalendar; }
            set { datePickerCalendar = value; }
        }

        private SpendingViewModel _spendingViewModel = new SpendingViewModel();
        public SpendingViewModel SpendingViewModel
        {
            get { return _spendingViewModel; }
            set { _spendingViewModel = value; }
        }

        private IncomeViewModel _incomeViewModel = new IncomeViewModel();
        public IncomeViewModel IncomeViewModel
        {
            get { return _incomeViewModel; }
            set { _incomeViewModel = value; }

        }


    }
    }


        

            

