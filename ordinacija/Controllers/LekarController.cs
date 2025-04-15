using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using webordinacija.Models;

namespace webordinacija.Controllers
{
    public class LekarController : Controller
    {
        public ActionResult Index(string sortOrder, string jmbgFilter, string imeFilter, string prezimeFilter, string statusFilter, string datumVremeFilter)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "lekar")
            {
                var termini = MvcApplication.Termini.FindAll(t => t.Lekar == model.Id);

                termini.ForEach(x =>
                {
                    if (x.Pacijent != 0)
                        x.PacijentPodaci = MvcApplication.Pacijenti.FirstOrDefault(p => p.Id == x.Pacijent);
                    else
                        x.PacijentPodaci = new Pacijent(0, "", "", "", "", "", "", DateTime.Now, "", new List<Termin>());
                });

                if (!string.IsNullOrEmpty(jmbgFilter))
                    termini = termini.Where(t => t.PacijentPodaci.JMBG.Contains(jmbgFilter)).ToList();
                if (!string.IsNullOrEmpty(imeFilter))
                    termini = termini.Where(t => t.PacijentPodaci.Ime.Contains(imeFilter)).ToList();
                if (!string.IsNullOrEmpty(prezimeFilter))
                    termini = termini.Where(t => t.PacijentPodaci.Prezime.Contains(prezimeFilter)).ToList();
                if (!string.IsNullOrEmpty(statusFilter))
                    termini = termini.Where(t => t.Status.Equals(statusFilter.ToUpper())).ToList();
                if (!string.IsNullOrEmpty(datumVremeFilter))
                {
                    DateTime.TryParse(datumVremeFilter, out DateTime datum);
                    termini = termini.Where(t => t.DatumIVremeD == datum).ToList();
                }
                ViewBag.CurrentSort = sortOrder;
                switch (sortOrder)
                {
                    case "status_desc":
                        termini = termini.OrderByDescending(t => t.Status).ToList();
                        break;
                    case "datumVreme":
                        termini = termini.OrderBy(t => t.DatumIVremeD).ToList();
                        break;
                    case "datumVreme_desc":
                        termini = termini.OrderByDescending(t => t.DatumIVremeD).ToList();
                        break;
                    case "jmbg":
                        termini = termini.OrderBy(t => t.PacijentPodaci.JMBG).ToList();
                        break;
                    case "jmbg_desc":
                        termini = termini.OrderByDescending(t => t.PacijentPodaci.JMBG).ToList();
                        break;
                    case "ime":
                        termini = termini.OrderBy(t => t.PacijentPodaci.Ime).ToList();
                        break;
                    case "ime_desc":
                        termini = termini.OrderByDescending(t => t.PacijentPodaci.Ime).ToList();
                        break;
                    case "prezime":
                        termini = termini.OrderBy(t => t.PacijentPodaci.Prezime).ToList();
                        break;
                    case "prezime_desc":
                        termini = termini.OrderByDescending(t => t.PacijentPodaci.Prezime).ToList();
                        break;
                    default:
                        termini = termini.OrderBy(t => t.Status).ToList();
                        break;
                }

                return View(termini);
            }
            else
            {
                return RedirectToAction("Index", "Main");
            }
        }

        public ActionResult Dodaj()
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "lekar")
                return View();
            else
                return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public ActionResult Dodavanje(DateTime termin_vreme)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "lekar")
            {
                if (termin_vreme != null)
                {
                    Termin termin = new Termin(DateTimeOffset.Now.ToUnixTimeSeconds(), model.Id, "SLOBODAN", termin_vreme.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), termin_vreme, 0, "");
                    Lekar lekar = MvcApplication.Lekari.FirstOrDefault(l => l.Id == model.Id);
                    termin.LekarIme = lekar.Ime;
                    termin.LekarPrezime = lekar.Prezime;
                    MvcApplication.Termini.Add(termin);
                    MvcApplication.Cuvanje();
                    return RedirectToAction("Index");
                }
                else
                    return RedirectToAction("Greska", new { poruka = "Niste uneli datum i vreme termina!" });
            }
            else
                return RedirectToAction("Index", "Main");
        }

        public ActionResult Pregled()
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "lekar")
            {
                var termini = MvcApplication.Termini.FindAll(t => t.Lekar == model.Id && t.Status == "ZAKAZAN");

                termini.ForEach(x =>
                {
                    if (x.Pacijent != 0)
                        x.PacijentPodaci = MvcApplication.Pacijenti.FirstOrDefault(p => p.Id == x.Pacijent);
                    else
                        x.PacijentPodaci = new Pacijent(0, "", "", "", "", "", "", DateTime.Now, "", new List<Termin>());
                });

                return View(termini);
            }
            else
                return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public ActionResult PrepisivanjeTerapije(long termin_id, string opis_terapije)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "lekar")
            {
                if (termin_id == 0 || string.IsNullOrEmpty(opis_terapije))
                    return RedirectToAction("Greska", new { poruka = "Termin ne postoji!" });

                Termin termin = MvcApplication.Termini.FirstOrDefault(t => t.Id == termin_id);

                if (termin != null)
                {
                    termin.OpisTerapije = opis_terapije;
                    MvcApplication.Cuvanje();
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index", "Main");
        }

        public ActionResult Greska(string poruka)
        {
            return View((object)poruka);
        }
    }
}