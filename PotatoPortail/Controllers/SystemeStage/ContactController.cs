using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers
{
    public class ContactController : Controller
    {
        private readonly BdPortail _db = new BdPortail();

        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Contact/Index.cshtml", _db.Contact.ToList());
        }     
        public ActionResult Modifier(int IdContact)
        {
            var lstEntreprise = new List<SelectListItem>();

            foreach (var entreprise in _db.Entreprise.ToList())
            {
                lstEntreprise.Add(new SelectListItem
                {
                    Text = entreprise.Nom,
                    Value = entreprise.IdEntreprise.ToString()
                });
            }
            ViewBag.Entreprises = lstEntreprise;

            var contact = _db.Contact.Find(IdContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View("~/Views/SystemeStage/Contact/Modifier.cshtml", contact);
        }

        [HttpPost]
        public ActionResult EnregistrerLesModifications(
            int? idContact,
            string nom,
            string courriel,
            string telephone,
            Entreprise entreprise
        )
        {
           
            if (idContact==null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
           
            var contact = _db.Contact.Find(idContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            contact.Courriel = courriel;
            contact.Nom = nom;
            contact.Telephone = telephone;
            contact.Entreprise_IdEntreprise = entreprise.IdEntreprise;

            _db.SaveChanges();

            return RedirectToAction("Index", "Contact");
        }

        public ActionResult Creation()
        {
            var lstEntreprise = new List<SelectListItem>();

            foreach (var entreprise in _db.Entreprise.ToList())
            {
                lstEntreprise.Add(new SelectListItem
                {
                    Text = entreprise.Nom,
                    Value = entreprise.IdEntreprise.ToString()
                });
            }

            ViewBag.Entreprises = lstEntreprise;

            var contact = new Contact
            {
                Courriel = "courriel@cegepjonquiere.ca",
                Nom = "Nouveau contact",
                Telephone = "123-456-7890",
                Entreprise = _db.Entreprise.First()               
            };

            _db.Contact.Add(contact);
            _db.SaveChanges(); 
            return View("~/Views/SystemeStage/Contact/Modifier.cshtml", contact);
        }

        public ActionResult Suppression(int? IdContact)
        {
            var contact = _db.Contact.Find(IdContact);

            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var stagesAyantCeContact = from stage in _db.Stage
                                       where stage.Contact.IdContact == IdContact
                                       select stage;

            if (!stagesAyantCeContact.Any())
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien �t� effectu�", ToastType.Success, true);
                _db.Contact.Remove(contact);
                _db.SaveChanges();
            }
            else
            {
                this.AddToastMessage("Confirmation de supression", "La supression n'a pas bien �t� effectu�", ToastType.Error, true);
            }

            return RedirectToAction("Index", "Contact");
        }
    }
}