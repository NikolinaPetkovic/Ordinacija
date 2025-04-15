using Common;
using Common.Models;
using System.Linq;
using System.Web.Mvc;

namespace webordinacija.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            var korisnik = Session["session"] as PrijavaModel;

            if (korisnik != null)
            {
                switch (korisnik.Rola)
                {
                    case "admin":
                        return RedirectToAction("Index", "Admin");
                    case "lekar":
                        return RedirectToAction("Index", "Lekar");
                    case "pacijent":
                        return RedirectToAction("Index", "Pacijent");
                    default:
                        return View();
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Prijava(PrijavaModel podaci)
        {
            if (string.IsNullOrEmpty(podaci.KorisnickoIme))
                return RedirectToAction("Greska", new { poruka = "Niste uneli korisnicko ime!" });
            else if (string.IsNullOrEmpty(podaci.Sifra))
                return RedirectToAction("Greska", new { poruka = "Niste uneli sifru!" });
            else
            {
                if (ValidacijaPrijave.PrijavaValidacija(
                    MvcApplication.Pacijenti.Cast<PrijavaModel>().ToList(),
                    MvcApplication.Lekari.Cast<PrijavaModel>().ToList(),
                    MvcApplication.Administratori.Cast<PrijavaModel>().ToList(),
                    podaci.KorisnickoIme, podaci.Sifra) is PrijavaModel model)
                {
                    Session["session"] = model;
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Greska", new { poruka = "Niste uneli postojeceg korisnika!" });
                }
            }
        }

        public ActionResult Odjava()
        {
            Session["session"] = null;
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Greska(string poruka)
        {
            return View((object)poruka);
        }
    }
}