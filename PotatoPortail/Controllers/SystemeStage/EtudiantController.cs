using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    public class EtudiantController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Etudiant/Index.cshtml", _db.Etudiant.ToList());
        }
        public ActionResult Modifier(int? IdEtudiant)
        {
            if (IdEtudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var etudiant = _db.Etudiant.Find(IdEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Etudiant/Modifier.cshtml", etudiant);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idEtudiant,
            string telephone,
            string prenom,
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

            var etudiant = _db.Etudiant.Find(idEtudiant);
            var poste = _db.Poste.Find(idPoste);
            var location = _db.Location.Find(idLocation);
            var entreprise = _db.Entreprise.Find(idEntreprise);

            if (etudiant == null || poste == null || location == null || entreprise == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            etudiant.Telephone = telephone;
            etudiant.Prenom = prenom;
            etudiant.Role = "Stagiaire";
            etudiant.CodePermanent = codePermanent;
            etudiant.CourrielEcole = courrielEcole;
            etudiant.CourrielPersonnel = courrielPersonnel;
            etudiant.NumeroDa = numeroDa;
            etudiant.NomDeFamille = nomDeFamille;

            etudiant.Preference.Poste = new List<Poste> {poste};
            etudiant.Preference.Entreprise = new List<Entreprise> {entreprise};
            etudiant.Preference.Location = new List<Location> {location};
            etudiant.Preference.Salaire = salaire;

            _db.SaveChanges();

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
            this.AddToastMessage("Confirmation de création", "La création a bien été effectué", ToastType.Success, true);
            _db.Etudiant.Add(etudiant);
            _db.SaveChanges();

            return View("~/Views/SystemeStage/Etudiant/Modifier.cshtml", etudiant);
        }
        public ActionResult ConsulterDossierEtudiant(int? IdEtudiant)
        {
            if (IdEtudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var etudiant = _db.Etudiant.Find(IdEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View("~/Views/SystemeStage/Etudiant/Actions/DossierEtudiant.cshtml", etudiant);
        }
        public ActionResult Suppression(int? IdEtudiant)
        {
            var etudiant = _db.Etudiant.Find(IdEtudiant);

            if (etudiant == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var applicationsParCetEtudiant = from application in _db.Application
                                             where application.Etudiant.IdEtudiant == IdEtudiant
                                             select application;

            if (!applicationsParCetEtudiant.Any())
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Success, true);
                _db.Etudiant.Remove(etudiant);
                _db.SaveChanges();
            }
            else
            {
                this.AddToastMessage("Confirmation de supression", "La supression n'a bien pas été effectué", ToastType.Error, true);
            }

            return RedirectToAction("Index", "Etudiant");
        }
    }
}