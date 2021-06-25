using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

#nullable disable

namespace WebApplication1
{
    public partial class Cars
    {
        public int CarId { get; set; }
        [Display(Name = "Владелец")]
        [Required]
        public int IdDriver { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        [RegularExpression("[A-HJ-NPR-Z0-9]{13}[0-9]{4}", ErrorMessage = "Неверный формат Vin номера!")]
        public string Vin { get; set; }
        [Display(Name = "Марка")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        public string Manufacturer { get; set; }
        [Display(Name = "Модель")]
        public string Model { get; set; }
        [Display(Name = "Год выпуска")]
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "Неверный формат года!")]
        public string Year { get; set; }
        [Display(Name = "Вес")]
        public double? Weight { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        [Display(Name = "Цвет")]
        public int? Color { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        [Display(Name = "Тип топлива")]
        public string EngineType { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        [Display(Name = "Тип привода")]
        public string TypeOfDrive { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым!")]
        [Display(Name = "Гос. номер")]
        public string StateNumber { get; set; }
        [Display(Name = "ДТП")]
        public bool? Dtp { get; set; }
        [Display(Name = "Дата ДТП")]
        [DataType(DataType.Date)]
        public DateTime? DateDtp { get; set; }
        [Display(Name = "Фото с места ДТП")]
        public string PhotoDtp { get; set; }
        [Display(Name = "Цвет")]
        public virtual CarColor ColorNavigation { get; set; }
        [Display(Name = "Владелец")]
        public virtual Drivers IdDriverNavigation { get; set; }
    }
}
