using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1
{
    public partial class Fines
    {
        public int Id { get; set; }
        public int IdDriver { get; set; }
        public int IdFine { get; set; }
        [Display(Name = "Дата штрафа")]
        [DataType(DataType.Date)]
        public DateTime? DateFine { get; set; }
        [Display(Name = "Фото")]
        public string PhotoFine { get; set; }

        public virtual Drivers IdDriverNavigation { get; set; }
        public virtual FinesList IdFineNavigation { get; set; }
    }
}
