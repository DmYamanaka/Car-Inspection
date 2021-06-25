using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WebApplication1
{
    public partial class Licence
    {
        [Display(Name = "Номер лицензии")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public int LicenceNumber { get; set; }
        [Display(Name = "Владелец лицензии")]
        public int? IdDriver { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата получения")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public DateTime? LicenceDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания срока")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public DateTime? ExpireDate { get; set; }
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string Categories { get; set; }
        [Display(Name = "Серия лицензии")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string LicenceSeries { get; set; }
        [Display(Name = "Владелец лицензии")]

        public virtual Drivers IdDriverNavigation { get; set; }
    }
}
