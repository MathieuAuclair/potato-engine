namespace ApplicationPlanCadre.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Programme")]
    public partial class Programme
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Programme()
        {
            PlanCadre = new HashSet<PlanCadre>();
        }

        [Key]
        public int idProgramme { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nom")]
        public string nom { get; set; }

        [Display(Name = "Nombre de sessions")]
        public int nbSession { get; set; }

        public string description
        {
            get
            {
                if (nom != null)
                    return nom + " � " + DevisMinistere.specialisation;
                else
                    return DevisMinistere.specialisation;
            }
        }

        [Required]
        [StringLength(4)]
        [Display(Name = "Ann�e")]
        [Range(1967, 2199, ErrorMessage = "L'ann�e est invalide. Le programme ne peux avoir �t� cr�� avant 1967.")]
        public string annee { get; set; }

        [Display(Name = "Derni�re validation")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? dateValidation { get; set; }

        public bool statusValider { get; set; }

        [Display(Name = "D�vis minist�riel")]
        public int idDevis { get; set; }

        public virtual DevisMinistere DevisMinistere { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanCadre> PlanCadre { get; set; }

    }
}
