using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class PrijavaModel
    {
        public long Id { get; set; } = 0;

        [JsonIgnore]
        public string Rola { get; set; }

        [Required]
        public string KorisnickoIme { get; set; }

        [Required]
        public string Sifra { get; set; }
    }
}