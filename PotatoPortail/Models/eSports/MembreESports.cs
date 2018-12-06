namespace PotatoPortail.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MembreESports
    {
        public MembreESports()
        {

        }

        [Display(Name = "ID �tudiant")]
        public string Id { get; set; }

        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Display(Name = "Pr�nom")]
        public string Prenom { get; set; }

        public virtual ICollection<Joueur> Joueur { get; set; }

        public virtual ICollection<Profil> Profil { get; set; }

        public string nomComplet
        {
            get
            {
                if (Prenom != null)
                    return Prenom + " " + Nom;
                else
                    return Nom;
            }
        }
    }
}
