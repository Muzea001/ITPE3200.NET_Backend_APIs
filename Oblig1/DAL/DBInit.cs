
using Oblig1.Models;
using System;

namespace Oblig1.DAL
{
    public class DBInit
    {

        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
            context.Database.EnsureCreated();


            if (!context.hus.Any())


            {

                var hus = new List<Hus>
                {
                    new Hus {husId=6,Addresse="Osloveien18",areal=200,Beskrivelse="bla bla bla", by="Oslo",eierID=7,erTilgjengelig=true,Pris=400,romAntall=4 },


                };
                context.AddRange(hus);
                context.SaveChanges();

            }
            var liste = new List<Ordre>();
            if (!context.kunde.Any())
            {

                var kunder = new List<Kunde>
                {

                  new Kunde("Mathias", new DateTime(20, 11, 1999), "Osloveien18", 40336208, "Methias99@gmail.com", 1, liste)
                };


                context.AddRange(kunder);
                context.SaveChanges();
            }

            if (!context.ordre.Any())
            {

                var ordre = new List<Ordre>
                {
                    new Ordre {
                        ordreId = 3,
                        Dato = DateTime.Now,
                        betaltGjennom = "Kort",
                        husID = 7,
                        hus = new Hus { husId = 2,Beskrivelse="Stort og Nyoppusset leilighet", Addresse="Osloveien22", areal=200, romAntall=4, erTilgjengelig=false },
                        kundeID = 1,
                        kunde = new Kunde("Mathias", new DateTime(20, 11, 1999), "Osloveien18", 40336208, "Methias99@gmail.com", 1, liste),
                        }
                        };


                context.AddRange(ordre);
                context.SaveChanges();
            };


            if (!context.bruker.Any())
            {

                var brukere = new List<Bruker>
                {
                    new Bruker {

                        Navn= "John Doe",
                        Fodselsdato = new DateTime(1990, 5, 15),
                        Addresse= "123 Main Street",
                        TelefonNmr= 5551234567,
                        Email= "johndoe@example.com",
                        Passord= "SecurePassword123",
                        erEier= true,
                        erAdmin= false
                        }
                     };

                context.AddRange(brukere);
                context.SaveChanges();
            }
}
    }
}


