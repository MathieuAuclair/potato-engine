using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PotatoPortail.Migrations;
using PotatoPortail.Models;
using PotatoPortail.Toast;

namespace PotatoPortail.Controllers.SystemeStage
{
    public class StageController : Controller
    {
       private readonly BdPortail _db = new BdPortail();

        public ActionResult Index()
        {
            return View("~/Views/SystemeStage/Stage/Index.cshtml", _db.Stage.ToList());
        }
        [HttpGet]
        public ActionResult Modifier(int? IdStage)
        {
            if (IdStage == null)
            {
                this.AddToastMessage("Confirmation de la modification", "La modification a échoué", ToastType.Error, true);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stage = _db.Stage.Find(IdStage);
          
         
            return View("~/Views/SystemeStage/Stage/Modifier.cshtml",stage);
        }
        public ActionResult Modifier(
            HttpPostedFileBase fichier,
            int? idLocation,
            int? idStatus,
            int? idPoste,
            int? idContact,
            int? numeroCivique,
            int? idStage,
            float? salaire,
            string nomRue,
            string ville,
            string province,
            string pays,
            string codePostal,
            string description,
            string nomDocument
        )
        {
            string nomFichier = null;

            if (fichier != null && fichier.ContentLength > 0)
            {
                nomFichier = Path.GetFileName(fichier.FileName) ?? string.Empty;
                fichier.SaveAs(Path.Combine(Server.MapPath("~/DescriptionStage"), nomFichier));
            }

            if (!EstCeQueLaRequeteContientLesParametresPourEdition())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (idStage == null)
            {
                this.AddToastMessage("", "La modification a échoué", ToastType.Error, true);

            }
            var stageInstance = _db.Stage.Find(idStage);
            var posteInstance = _db.Poste.Find(idPoste);
            var contactInstance = _db.Contact.Find(idContact);
            var statusInstance = _db.StatutStage.Find(idStatus);
            var locationInstance = _db.Location.Find(idLocation);

            stageInstance.Poste = posteInstance;
            stageInstance.Contact = contactInstance;
            stageInstance.StatutStage = statusInstance;
            stageInstance.Location = locationInstance;
            stageInstance.Description = description;
            stageInstance.NomDocument = nomFichier;
            stageInstance.CodePostal = codePostal;
            stageInstance.Salaire = salaire ?? 0.0f;

            this.AddToastMessage("Confirmation", "l'opération s'est effectué avec succes", ToastType.Success, true);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        private bool EstCeQueLaRequeteContientLesParametresPourEdition()
        {
            return (
                Request.Form["idPoste"] != null &&
                Request.Form["idContact"] != null &&
                Request.Form["idStatus"] != null &&
                Request.Form["idLocation"] != null &&
                Request.Form["idStage"] != null &&
                Request.Form["description"] != null &
                Request.Form["pays"] != null &&
                Request.Form["province"] != null &&
                Request.Form["ville"] != null &&
                Request.Form["rue"] != null &&
                Request.Form["numeroCivique"] != null &&
                Request.Form["codePostal"] != null &&
                Request.Form["salaire"] != null
            );
        }
        [HttpPost]
        public ActionResult TeleverserFichier()
        {
            var nomDeFichier = Request.Form["fichier"];

            if (string.IsNullOrEmpty(nomDeFichier))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("content-disposition", "attachement;filename=" + nomDeFichier);
            Response.TransmitFile(Server.MapPath("~/DescriptionStage/" + nomDeFichier));
            Response.End();

            return View("~/Views/SystemeStage/Stage/Index.cshtml");
        }
        public ActionResult AjouterStage()
        {
            var Stage = new Stage
            {
                Location = _db.Location.First(),
                NumeroCivique = 0,
                NomRue = "nom de rue",
                Ville = "Saguenay",
                Province = "Québec",
                Pays = "Canada",
                CodePostal = "G7X7W2",
                Poste = _db.Poste.First(),
                StatutStage = _db.StatutStage.First(),
                Contact = _db.Contact.First(),
                Description = "Description du Stage",
                NomDocument = "",
                Salaire = 0,
                
            };
            this.AddToastMessage("Confirmation de création", "La création a bien été effectué", ToastType.Success, true);
            _db.Stage.Add(Stage);
            _db.SaveChanges();

            return View("~/Views/SystemeStage/Stage/Modifier.cshtml", Stage);
        }
        public ActionResult Suppression(int? IdStage)
        {
            var Stage = _db.Stage.Find(IdStage);

            if (Stage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var applicationsPourCeStage = from application in _db.Application
                                          where application.Stage.IdStage == IdStage
                                          select application;

            if (!applicationsPourCeStage.Any())
            {
                this.AddToastMessage("Confirmation de supression", "La supression a bien été effectué", ToastType.Success, true);
                _db.Stage.Remove(Stage);
                _db.SaveChanges();
            }
            else { this.AddToastMessage("Confirmation de supression", "La supression n'a bien pas été effectué", ToastType.Error, true); }

            return RedirectToAction("Index", "Stage");
        }
    }
}