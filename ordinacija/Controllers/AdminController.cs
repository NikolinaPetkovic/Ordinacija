using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using webordinacija.Models;

namespace webordinacija.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index(string sortOrder, string korisnickoImeFilter, string jmbgFilter, string imeFilter, string prezimeFilter, string datumRodjenjaFilter, string elektronskaPostaFilter)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "admin")
            {
                var pacijenti = MvcApplication.Pacijenti.AsQueryable();

                if (!string.IsNullOrEmpty(korisnickoImeFilter))
                    pacijenti = pacijenti.Where(p => p.KorisnickoIme.Contains(korisnickoImeFilter));
                if (!string.IsNullOrEmpty(jmbgFilter))
                    pacijenti = pacijenti.Where(p => p.JMBG.Contains(jmbgFilter));
                if (!string.IsNullOrEmpty(imeFilter))
                    pacijenti = pacijenti.Where(p => p.Ime.Contains(imeFilter));
                if (!string.IsNullOrEmpty(prezimeFilter))
                    pacijenti = pacijenti.Where(p => p.Prezime.Contains(prezimeFilter));
                if (!string.IsNullOrEmpty(datumRodjenjaFilter) && DateTime.TryParse(datumRodjenjaFilter, out DateTime datum))
                    pacijenti = pacijenti.Where(p => p.DatumRodjenja.AsDateTime() == datum);
                if (!string.IsNullOrEmpty(elektronskaPostaFilter))
                    pacijenti = pacijenti.Where(p => p.ElektronskaPosta.Contains(elektronskaPostaFilter));

                ViewBag.CurrentSort = sortOrder;

                switch (sortOrder)
                {
                    case "jmbg_desc":
                        pacijenti = pacijenti.OrderByDescending(p => p.JMBG);
                        break;
                    case "ime":
                        pacijenti = pacijenti.OrderBy(p => p.Ime);
                        break;
                    case "ime_desc":
                        pacijenti = pacijenti.OrderByDescending(p => p.Ime);
                        break;
                    case "prezime":
                        pacijenti = pacijenti.OrderBy(p => p.Prezime);
                        break;
                    case "prezime_desc":
                        pacijenti = pacijenti.OrderByDescending(p => p.Prezime);
                        break;
                    case "datumRodjenja":
                        pacijenti = pacijenti.OrderBy(p => DateTime.Parse(p.DatumRodjenja, CultureInfo.InvariantCulture));
                        break;
                    case "datumRodjenja_desc":
                        pacijenti = pacijenti.OrderByDescending(p => DateTime.Parse(p.DatumRodjenja, CultureInfo.InvariantCulture));
                        break;
                    case "elektronskaPosta":
                        pacijenti = pacijenti.OrderBy(p => p.ElektronskaPosta);
                        break;
                    case "elektronskaPosta_desc":
                        pacijenti = pacijenti.OrderByDescending(p => p.ElektronskaPosta);
                        break;
                    default:
                        pacijenti = pacijenti.OrderBy(p => p.KorisnickoIme);
                        break;
                }

                return View(pacijenti.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }
        }

        public ActionResult Dodaj()
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "admin")
                return View();
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DodajPacijenta(Pacijent pacijent)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "admin")
            {
                List<Pacijent> pacijenti = MvcApplication.Pacijenti;

                string poruka = "";

                if (pacijenti.Exists(p => p.KorisnickoIme == pacijent.KorisnickoIme))
                    poruka = "Korisnicko ime je zauzeto!";

                if (pacijenti.Exists(p => p.ElektronskaPosta == pacijent.ElektronskaPosta))
                    poruka = "Elektronska posta je zauzeta!";
                if (pacijenti.Exists(p => p.JMBG == pacijent.JMBG))
                    poruka = "JMBG je zauzet!";

                if (!long.TryParse(pacijent.JMBG, out long jmbg))
                    poruka = "JMBG nije u validnom brojevnom formatu!";

                if (poruka != "")
                    return RedirectToAction("Greska", new { poruka });

                pacijent.Id = jmbg;
                pacijent.DatumRodjenja = pacijent.DatumRodjenjaD.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                pacijenti.Add(pacijent);

                MvcApplication.Kartoni.Add(new ZdravstveniKarton(jmbg, new List<long>(), pacijent.Id));
                MvcApplication.Cuvanje();
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index", "Main");
        }

        public ActionResult Izmeni(string korisnickoIme)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "admin")
            {
                var pacijent = MvcApplication.Pacijenti.FirstOrDefault(p => p.KorisnickoIme == korisnickoIme);
                if (pacijent == null)
                    return RedirectToAction("Index");

                return View(pacijent);
            }
            else
                return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public ActionResult Izmena(Pacijent pacijent)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "admin")
            {
                if (!MvcApplication.Pacijenti.Exists(p => p.Id == pacijent.Id))
                    return RedirectToAction("Greska", new { poruka = "Pacijent nije pronadjen." });

                if (MvcApplication.Pacijenti.Exists(p => p.Id != pacijent.Id && p.ElektronskaPosta == pacijent.ElektronskaPosta))
                    return RedirectToAction("Greska", new { poruka = "Adresa elektronske poste je vec zauzeta." });

                var izmena = MvcApplication.Pacijenti.FirstOrDefault(p => p.Id == pacijent.Id);
                izmena.Ime = pacijent.Ime;
                izmena.Prezime = pacijent.Prezime;
                izmena.DatumRodjenjaD = pacijent.DatumRodjenjaD;
                izmena.DatumRodjenja = pacijent.DatumRodjenjaD.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                izmena.ElektronskaPosta = pacijent.ElektronskaPosta;
                MvcApplication.Cuvanje();
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index", "Main");
        }

        public ActionResult Brisanje(string korisnickoIme)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "admin")
            {
                if (MvcApplication.Pacijenti.Exists(p => p.KorisnickoIme == korisnickoIme))
                {
                    Pacijent pronadjen = MvcApplication.Pacijenti.FirstOrDefault(p => p.KorisnickoIme == korisnickoIme);

                    if (pronadjen != null)
                    {
                        MvcApplication.Pacijenti.Remove(pronadjen);
                        MvcApplication.Termini.RemoveAll(t => t.Pacijent == pronadjen.Id);
                        MvcApplication.Kartoni.RemoveAll(t => t.BrojZdravstevnogOsiguranjaPacijenta == pronadjen.Id);
                        MvcApplication.Cuvanje();
                        return RedirectToAction("Index");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Greska(string poruka)
        {
            return View((object)poruka);
        }

    }
}
