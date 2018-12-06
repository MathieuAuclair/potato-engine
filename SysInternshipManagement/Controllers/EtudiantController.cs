using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SysInternshipManagement.Migrations;
using SysInternshipManagement.Models;

namespace SysInternshipManagement.Controllers
{
    public class EtudiantController : Controller
    {
        private readonly DatabaseContext _bd = new DatabaseContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View( _bd.etudiant.ToList());
        }

     //   [HttpPost]
        public ActionResult Modifier(int? IdEtudiant)
        {
            if (IdEtudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var etudiant = _bd.etudiant.Find(IdEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(etudiant);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idEtudiant,
            string telephone,
            string prenom,
            string role,
            string codePermanent,
            string courrielEcole,
            string courrielPersonnel,
            string numeroDa,
            string nomDeFamille,
            int? idLocation,
            int? idEntreprise,
            int? idPoste,
            float salaire
        )
        {
            if (idEtudiant == null || idEntreprise == null || idLocation == null || idPoste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var etudiant = _bd.etudiant.Find(idEtudiant);
            var poste = _bd.poste.Find(idPoste);
            var location = _bd.location.Find(idLocation);
            var entreprise = _bd.entreprise.Find(idEntreprise);

            if (etudiant == null || poste == null || location == null || entreprise == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            etudiant.Telephone = telephone;
            etudiant.Prenom = prenom;
            etudiant.Role = role;
            etudiant.CodePermanent = codePermanent;
            etudiant.CourrielEcole = courrielEcole;
            etudiant.CourrielPersonnel = courrielPersonnel;
            etudiant.NumeroDa = numeroDa;
            etudiant.NomDeFamille = nomDeFamille;

            etudiant.Preference.Poste = new List<Poste> {poste};
            etudiant.Preference.Entreprise = new List<Entreprise> {entreprise};
            etudiant.Preference.Location = new List<Location> {location};
            etudiant.Preference.Salaire = salaire;

            _bd.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Creation()
        {
            var etudiant = new Etudiant
            {
                Telephone = "123-456-7890",
                Preference = new Preference
                {
                    Salaire = 0,
                },
                Prenom = "prénom",
                NomDeFamille = "nom de famille",
                Role = "etudiant",
                CodePermanent = "ABCD12345678",
                CourrielEcole = "email@cegepjonquiere.ca",
                CourrielPersonnel = "email@cegepjonquiere.ca",
                NumeroDa = "1234567",
            };

            _bd.etudiant.Add(etudiant);
            _bd.SaveChanges();

            return View("~/Views/Etudiant/Modifier.cshtml", etudiant);
        }

        [HttpPost]
        public ActionResult ConsulterDossierEtudiant(int? IdEtudiant)
        {
            if (IdEtudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var etudiant = _bd.etudiant.Find(IdEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/Etudiant/Actions/DossierEtudiant.cshtml", etudiant);
        }

        public ActionResult Suppression(int? IdEtudiant)
        {
            var etudiant = _bd.etudiant.Find(IdEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var applicationsParCetEtudiant = from application in _bd.application
                                             where application.Etudiant.IdEtudiant == IdEtudiant
                                             select application;

            if (!applicationsParCetEtudiant.Any())
            {
                _bd.etudiant.Remove(etudiant);
                _bd.SaveChanges();
            }

            return RedirectToAction("Index", "Etudiant");
        }
    }
}