namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Caracteristique
    {
        public Caracteristique()
        {
            
        }

        public int Id { get; set; }

        [Required]
        public string NomCaracteristique { get; set; }

        [Display(Name = "Jeu")]
        public int IdJeu { get; set; }

        public virtual Jeu Jeu { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
