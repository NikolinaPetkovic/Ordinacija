using Newtonsoft.Json;
using System;

namespace webordinacija.Models
{
    public class Termin
    {
        public long Id { get; set; }

        public long Lekar { get; set; }

        public string Status { get; set; } = "SLOBODAN";

        public string DatumIVreme { get; set; }

        public DateTime DatumIVremeD { get; set; }

        public long Pacijent { get; set; } = 0;

        public string OpisTerapije { get; set; }

        public string LekarIme { get; set; }

        public string LekarPrezime { get; set; }


        [JsonIgnore]
        public Pacijent PacijentPodaci { get; set; }

        public Termin() { }

        public Termin(long id, long lekar, string status, string datumIVreme, DateTime datumIVremeD, long pacijent, string opisTerapije)
        {
            Id = id;
            Lekar = lekar;
            Status = status;
            DatumIVreme = datumIVreme;
            DatumIVremeD = datumIVremeD;
            Pacijent = pacijent;
            OpisTerapije = opisTerapije;
        }
    }
}