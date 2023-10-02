
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
                    



                };
                context.AddRange(person);
                context.SaveChanges();

            }

            if (!context.bruker.Any())
            {

                var brukere = new List<Bruker>
                {
                   new Bruker {personID=1,Navn="Jorgen",Addresse="Bjerkeveien21",Email="Jorgen12@gmail.com",Fodselsdato= new DateTime(1989,6,2),TelefonNmr=31625400,Passord="hei123",erAdmin=false,erEier=true,}
                     };

                context.AddRange(brukere);
                context.SaveChanges();
            }

            if (!context.kunde.Any())
            {

                var kunder = new List<Kunde>
                {

                
                };


                context.AddRange(kunder);
                context.SaveChanges();
            }

            if (!context.eier.Any())
            {

                var eier = new Eier
                {
                    personID = 1,
                    Navn = "Ali",
                    Addresse = "Gronlandveien22",
                    Email = "Gronland13@gmail.com",
                    Fodselsdato = new DateTime(1981, 2, 11),
                    TelefonNmr = 40306080,
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
                    new Hus {husId=1,Addresse="Osloveien18",areal=200,Beskrivelse="bla bla bla", by="Oslo",erTilgjengelig=true,Pris=400,romAntall=4, erMoblert=true, harParkering=false, eierID=1, bildeURL = "~/Bilder/1.jpg"},
                    new Hus {husId =2,Addresse="Osloveien22", areal=250, Beskrivelse="bla bla ",by="Oslo",erTilgjengelig=true,Pris=200,romAntall=3,erMoblert=false,harParkering=true,eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=3, Addresse="Trondheimsgate 10", areal=220, Beskrivelse="Spacious house with a view of the river.", by="Trondheim", erTilgjengelig=true, Pris=750, romAntall=6, erMoblert=false, harParkering=true, eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=4, Addresse="Stavangerveien 5", areal=160, Beskrivelse="Modern townhouse in a quiet neighborhood.", by="Stavanger", erTilgjengelig=true, Pris=500, romAntall=4, erMoblert=true, harParkering=false, eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=5, Addresse="Bergensgate 22", areal=180, Beskrivelse="A cozy family home with a garden.", by="Bergen", erTilgjengelig=true, Pris=600, romAntall=5, erMoblert=true, harParkering=true, eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=6, Addresse="Bodogate 8", areal=190, Beskrivelse="Rustic cottage in the countryside.", by="Bodo", erTilgjengelig=true, Pris=450, romAntall=3, erMoblert=true, harParkering=false, eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=7, Addresse="Tromsogate 15", areal=240, Beskrivelse="Spacious family home with a view of the mountains.", by="Tromso", erTilgjengelig=true, Pris=950, romAntall=8, erMoblert=true, harParkering=true, eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=8, Addresse="Bergengate 30", areal=170, Beskrivelse="Cozy apartment in the city center.", by="Bergen", erTilgjengelig=true, Pris=700, romAntall=4, erMoblert=true, harParkering=false, eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=9, Addresse="Kristiansandveien 7", areal=200, Beskrivelse="Modern duplex with a rooftop terrace.", by="Kristiansand", erTilgjengelig=true, Pris=800, romAntall=6, erMoblert=false, harParkering=true, eierID=1, bildeURL="~/Bilder/1.jpg"},
                    new Hus {husId=10, Addresse="Alesundgate 25", areal=185, Beskrivelse="Quaint cottage by the seaside.", by="Alesund", erTilgjengelig=true, Pris=550, romAntall=5, erMoblert=false, harParkering=true, eierID=1, bildeURL="~/Bilder/1.jpg"},
               


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


