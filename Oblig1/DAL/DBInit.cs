
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


            if (!context.person.Any())


            {

                var person = new List<Person>
                {
                    new Person {personID=1,Navn="Mathias",Addresse="Osloveien18",Email="Mathias18@gmail.com",Fodselsdato= new DateTime(1999,5,8),TelefonNmr=14789531 },
                    new Person {personID=2,Navn="Ole",Addresse="Osloveien20",Email="Osloveien20",Fodselsdato= new DateTime(1996,2,8),TelefonNmr=95632164 }


                };
                context.AddRange(person);
                context.SaveChanges();

            }

            if (!context.bruker.Any())
            {

                var brukere = new List<Bruker>
                {
                   new Bruker {personID=1,Navn="Mathias",Addresse="Osloveien18",Email="Mathias18@gmail.com",Fodselsdato= new DateTime(1999,5,8),TelefonNmr=14789531,Passord="hei123",erAdmin=false,erEier=true,}
                     };

                context.AddRange(brukere);
                context.SaveChanges();
            }

            if (!context.kunde.Any())
            {

                var kunder = new List<Kunde>
                {

                  new Kunde{personID=2,Navn="Ole",Addresse="Osloveien20",Email="Ole20@gmail.com",Fodselsdato= new DateTime(1996,2,8),TelefonNmr=95632164,kundeId=1 }
                };


                context.AddRange(kunder);
                context.SaveChanges();
            }

            if (!context.eier.Any())
            {

                var eier = new Eier
                {
                    personID = 1,
                    Navn = "Mathias",
                    Addresse = "Osloveien18",
                    Email = "Mathias18@gmail.com",
                    Fodselsdato = new DateTime(1999, 5, 8),
                    TelefonNmr = 14789531,
                    eierID = 1,
                    kontoNummer = 11111111111
                };
                context.AddRange(eier);
                context.SaveChanges();

            }



            if (!context.hus.Any())


            {

                var hus = new List<Hus>
                {
                    new Hus {husId=1,Addresse="Osloveien18",areal=200,Beskrivelse="bla bla bla", by="Oslo",erTilgjengelig=true,Pris=400,romAntall=4, erMoblert=true, harParkering=false, eierID=1, bildeURL = "" },


                };
                context.AddRange(hus);
                context.SaveChanges();

            }
            
          

            if (!context.ordre.Any())
            {

                var ordre = new List<Ordre>
                {
                    new Ordre {
                        ordreId = 1,
                        Dato = DateTime.Now,
                        betaltGjennom = "Kort",
                        husID = 1,
                        kundeID = 1,
                        }
                };


                context.AddRange(ordre);
                context.SaveChanges();
            };

}
    }
}


