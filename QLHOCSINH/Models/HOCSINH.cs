//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLHOCSINH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HOCSINH
    {
        public string MaHS { get; set; }
        public string HoTen { get; set; }
        public bool GioiTinh { get; set; }
        public System.DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public double DiemTB { get; set; }
        public string MaLop { get; set; }
    
        public virtual LOP LOP { get; set; }
    }
}
