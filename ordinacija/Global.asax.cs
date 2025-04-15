using Common;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using webordinacija.Models;

namespace webordinacija
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static List<Administrator> Administratori = new List<Administrator>();
        public static List<Lekar> Lekari = new List<Lekar>();
        public static List<Pacijent> Pacijenti = new List<Pacijent>();
        public static List<Termin> Termini = new List<Termin>();
        public static List<ZdravstveniKarton> Kartoni = new List<ZdravstveniKarton>();
        public static string app_data = HostingEnvironment.MapPath(@"~/App_Data/");

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            JsonLoader.Ucitavanje(app_data + "administratori.json", ref Administratori);
            JsonLoader.Ucitavanje(app_data + "lekari.json", ref Lekari);
            JsonLoader.Ucitavanje(app_data + "pacijenti.json", ref Pacijenti);
            JsonLoader.Ucitavanje(app_data + "termini.json", ref Termini);
            JsonLoader.Ucitavanje(app_data + "kartoni.json", ref Kartoni);
        }

        public static void Cuvanje()
        {
            JsonSaver.Cuvanje(app_data + "administratori.json", Administratori);
            JsonSaver.Cuvanje(app_data + "lekari.json", Lekari);
            JsonSaver.Cuvanje(app_data + "pacijenti.json", Pacijenti);
            JsonSaver.Cuvanje(app_data + "termini.json", Termini);
            JsonSaver.Cuvanje(app_data + "kartoni.json", Kartoni);
        }
    }
}
