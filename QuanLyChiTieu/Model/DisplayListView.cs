﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieu.Model
{
    public class DisplayListView
    {
        public int ID { get; set; }
        public int STT { get; set; }
        public string Danhmuc { get; set; }
        public decimal Sotien { get; set; }
        public DateTime Thoigian { get; set; }
        public string Chitiet { get; set; }
    }
}
