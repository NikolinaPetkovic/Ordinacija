using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using webordinacija.Models;

namespace webordinacija.Controllers
{
    public class PacijentController : Controller
    {
        public ActionResult Index(string sortOrder, string imeLekaraFilter, string prezimeLekaraFilter, string statusFilter, string datumVremeFilter)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "pacijent")
            {
                var termini = MvcApplication.Termini.FindAll(t => t.Status == "SLOBODAN").ToList();

                if (!string.IsNullOrEmpty(imeLekaraFilter))
                    termini = termini.Where(t => t.LekarIme.Contains(imeLekaraFilter)).ToList();
                if (!string.IsNullOrEmpty(prezimeLekaraFilter))
                    termini = termini.Where(t => t.LekarPrezime.Contains(prezimeLekaraFilter)).ToList();
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
                    case "imeLekara":
                        termini = termini.OrderBy(t => t.LekarIme).ToList();
                        break;
                    case "imeLekara_desc":
                        termini = termini.OrderByDescending(t => t.LekarIme).ToList();
                        break;
                    case "prezimeLekara":
                        termini = termini.OrderBy(t => t.LekarPrezime).ToList();
                        break;
                    case "prezimeLekara_desc":
                        termini = termini.OrderByDescending(t => t.LekarPrezime).ToList();
                        break;
                    default:
                        termini = termini.OrderBy(t => t.Status).ToList();
                        break;
                }

                return View(termini);
            }
            else
                return RedirectToAction("Index", "Main");
        }


        public ActionResult Pregled()
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "pacijent")
            {
                var termini = MvcApplication.Termini.FindAll(t => t.Pacijent == model.Id && t.Status == "ZAKAZAN");

                return View(termini);
            }
            else
                return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public ActionResult Zakazi(int id)
        {
            if (Session["session"] is PrijavaModel model && model.Rola == "pacijent")
            {
                if (MvcApplication.Termini.Exists(t => t.Id == id && t.Status == "SLOBODAN"))
                {
                    Termin zakazivanje = MvcApplication.Termini.FirstOrDefault(t => t.Id == id);

                    if (zakazivanje != null)
                    {
                        zakazivanje.Pacijent = model.Id;
                        zakazivanje.Status = "ZAKAZAN";

                        ZdravstveniKarton karton = MvcApplication.Kartoni.FirstOrDefault(k => k.BrojZdravstevnogOsiguranjaPacijenta == model.Id);

                        if (karton != null)
                            karton.ListaTermina.Add(zakazivanje.Id);
                        else
                            MvcApplication.Kartoni.Add(
                                new ZdravstveniKarton(model.Id, new List<long>() { zakazivanje.Id }, model.Id));

                        MvcApplication.Cuvanje();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Greska", new { poruka = "Termin ne postoji!" });
                    }
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