using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webordinacija.Models
{
    public class Pacijent : PrijavaModel
    {
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string JMBG { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        public string DatumRodjenja { get; set; }

        public DateTime DatumRodjenjaD { get; set; }

        [Required]
        [EmailAddress]
        public string ElektronskaPosta { get; set; }

        public List<Termin> ZakazaniTermini { get; set; } = new List<Termin>();

        public Pacijent() { }

        public Pacijent(long id, string korisnickoIme, string jMBG, string sifra, string ime, string prezime, string datumRodjenja, DateTime datumRodjenjaD, string elektronskaPosta, List<Termin> zakazaniTermini)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            JMBG = jMBG;
            Sifra = sifra;
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            DatumRodjenjaD = datumRodjenjaD;
            ElektronskaPosta = elektronskaPosta;
            ZakazaniTermini = zakazaniTermini;
        }
    }
}