using Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class ValidacijaPrijave
    {
        public static PrijavaModel PrijavaValidacija(List<PrijavaModel> pacijenti, List<PrijavaModel> lekari,
            List<PrijavaModel> admin, string korisnickoIme, string sifra)
        {
            if (pacijenti.Exists(p => p.KorisnickoIme == korisnickoIme && p.Sifra == sifra))
            {
                var pronadjen = pacijenti.First(p => p.KorisnickoIme == korisnickoIme && p.Sifra == sifra);
                return new PrijavaModel() { Id = pronadjen.Id, Rola = "pacijent" };
            }
            else if (lekari.Exists(l => l.KorisnickoIme == korisnickoIme && l.Sifra == sifra))
            {
                var pronadjen = lekari.First(l => l.KorisnickoIme == korisnickoIme && l.Sifra == sifra);
                return new PrijavaModel() { Id = pronadjen.Id, Rola = "lekar" };
            }
            else if (admin.Exists(a => a.KorisnickoIme == korisnickoIme && a.Sifra == sifra))
            {
                return new PrijavaModel() { Rola = "admin" };
            }

            return null;
        }
    }
}
