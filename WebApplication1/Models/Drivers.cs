using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1
{
    public partial class Drivers
    {
        public Drivers()
        {
            Cars = new HashSet<Cars>();
            Fines = new HashSet<Fines>();
            Licence = new HashSet<Licence>();
        }

        public int Id { get; set; }
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string Name { get; set; }
        [Display(Name = "Серия паспорта")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public int? PassportSerial { get; set; }
        [Display(Name = "Номер паспорта")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public int PassportNumber { get; set; }
        [Display(Name = "Адресс проживания")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string Address { get; set; }
        [Display(Name = "Место работы")]
        public string Company { get; set; }
        [Display(Name = "Должность")]
        public string Jobname { get; set; }
        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Фото")]
        public string Photo { get; set; }

        public virtual ICollection<Cars> Cars { get; set; }
        public virtual ICollection<Fines> Fines { get; set; }
        public virtual ICollection<Licence> Licence { get; set; }
    }
}
