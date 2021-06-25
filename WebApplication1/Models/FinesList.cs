using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1
{
    public partial class FinesList
    {
        public FinesList()
        {
            Fines = new HashSet<Fines>();
        }

        public int Id { get; set; }
        [Display(Name="Размер штрафа")]
        public int? Fine { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<Fines> Fines { get; set; }
    }
}
