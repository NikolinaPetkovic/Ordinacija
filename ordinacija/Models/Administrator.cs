using Common.Models;

namespace webordinacija.Models
{
    public class Administrator : PrijavaModel
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string DatumRodjenja { get; set; }

        public Administrator() { }

        public Administrator(string korisnickoIme, string sifra, string ime, string prezime, string datumRodjenja)
        {
            KorisnickoIme = korisnickoIme;
            Sifra = sifra;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
        }
    }
}