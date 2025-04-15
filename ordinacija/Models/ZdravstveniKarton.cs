using System.Collections.Generic;

namespace webordinacija.Models
{
    public class ZdravstveniKarton
    {
        public long Id { get; set; }

        public List<long> ListaTermina { get; set; } = new List<long>();

        public long BrojZdravstevnogOsiguranjaPacijenta { get; set; }

        public ZdravstveniKarton() { }

        public ZdravstveniKarton(long id, List<long> listaTermina, long brojZdravstevnogOsiguranjaPacijenta)
        {
            Id = id;
            ListaTermina = listaTermina;
            BrojZdravstevnogOsiguranjaPacijenta = brojZdravstevnogOsiguranjaPacijenta;
        }
    }
}