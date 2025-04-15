using Common.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webordinacija.Models
{
    public class Lekar : PrijavaModel
    {
        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        [RegularExpression(@"\d{2}/\d{2}/\d{4}")]
        public string DatumRodjenja { get; set; }

        [Required]
        [EmailAddress]
        public string ElektronskaPosta { get; set; }

        public List<Termin> ZakazaniISlobodniTermini { get; set; } = new List<Termin>();

        public Lekar() { }

        public Lekar(long id, string korisnickoIme, string sifra, string ime, string prezime, string datumRodjenja, string elektronskaPosta, List<Termin> zakazaniISlobodniTermini)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            Sifra = sifra;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            ElektronskaPosta = elektronskaPosta;
            ZakazaniISlobodniTermini = zakazaniISlobodniTermini;
        }
    }
}