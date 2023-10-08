

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
            
            


            if (!context.person.Any() && !context.kunde.Any() && !context.eier.Any())


            {

                var person = new Person { 
                
                 Navn="Mathias",Addresse="Osloveien18",Email="Mathias18@gmail.com",Fodselsdato= new DateTime(1999,5,8),TelefonNmr=14789531
                  
                };

                var kunde = new Kunde
                 {
                    Person = person
               
                };
                var eier = new Eier
                {
                    Person = person,
                    kontoNummer = 11111111111,
                    antallAnnonser = 0
                };

 


                context.AddRange(kunde,eier);
                context.SaveChanges();
            }

            


            if (!context.hus.Any())

            {
                var hus = new List<Hus>
                {
                    new Hus {Addresse="Osloveien18",areal=200,Beskrivelse="bla bla bla", by="Oslo",erTilgjengelig=true,Pris=400,romAntall=4, erMoblert=true, harParkering=false, bildeURL = "~/Bilder/1.jpg"},
                    new Hus {Addresse="Osloveien22", areal=250, Beskrivelse="bla bla ",by="Oslo",erTilgjengelig=true,Pris=200,romAntall=3,erMoblert=false,harParkering=true, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Trondheimsgate 10", areal=220, Beskrivelse="Spacious house with a view of the river.", by="Trondheim", erTilgjengelig=true, Pris=750, romAntall=6, erMoblert=false, harParkering=true, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Stavangerveien 5", areal=160, Beskrivelse="Modern townhouse in a quiet neighborhood.", by="Stavanger", erTilgjengelig=true, Pris=500, romAntall=4, erMoblert=true, harParkering=false, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Bergensgate 22", areal=180, Beskrivelse="A cozy family home with a garden.", by="Bergen", erTilgjengelig=true, Pris=600, romAntall=5, erMoblert=true, harParkering=true, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Bodogate 8", areal=190, Beskrivelse="Rustic cottage in the countryside.", by="Bodo", erTilgjengelig=true, Pris=450, romAntall=3, erMoblert=true, harParkering=false, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Tromsogate 15", areal=240, Beskrivelse="Spacious family home with a view of the mountains.", by="Tromso", erTilgjengelig=true, Pris=950, romAntall=8, erMoblert=true, harParkering=true, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Bergengate 30", areal=170, Beskrivelse="Cozy apartment in the city center.", by="Bergen", erTilgjengelig=true, Pris=700, romAntall=4, erMoblert=true, harParkering=false, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Kristiansandveien 7", areal=200, Beskrivelse="Modern duplex with a rooftop terrace.", by="Kristiansand", erTilgjengelig=true, Pris=800, romAntall=6, erMoblert=false, harParkering=true, bildeURL="~/Bilder/1.jpg"},
                    new Hus {Addresse="Alesundgate 25", areal=185, Beskrivelse="Quaint cottage by the seaside.", by="Alesund", erTilgjengelig=true, Pris=550, romAntall=5, erMoblert=false, harParkering=true, bildeURL="~/Bilder/1.jpg"},



            };

                var person = new Person
                {
                    
                    Navn = "Ole",
                    Addresse = "Osloveien22",
                    Email = "Jonas13@gmail.com",
                    Fodselsdato = new DateTime(1985, 7, 4),
                    TelefonNmr = 87546611,
                   
                };

                var eier = new Eier
                {
                    Person = person,
                    kontoNummer = 22222222222,
                    antallAnnonser = 0,
                    husListe = hus
                };


                context.AddRange(eier);
                context.SaveChanges();

            }



            if (!context.ordre.Any())
            {
                var person = new Person
                {
                    Navn = "Marius",
                    Addresse = "Hammerveien22",
                    Email = "Marius22@gmail.com",
                    Fodselsdato = new DateTime(2001, 4, 3),
                    TelefonNmr = 14789531
                };

                

                var kunde = new Kunde
                {
                    Person = person
                };

                context.Add(kunde);
                context.SaveChanges();

                var ordre = new List<Ordre>
    {
        new Ordre
        {
            Dato = DateTime.Now,
            betaltGjennom = "Kort",
            kundeID = kunde.kundeID  // Assigning the foreign key for Kunde
        },
        new Ordre
        {
            Dato = DateTime.Now,
            betaltGjennom = "Klarna",
            kundeID = kunde.kundeID  // Assigning the foreign key for Kunde
        }
    };

                var eier = new Eier
                {
                    Person = person,
                    kontoNummer = 33333333333,
                    antallAnnonser = 0
                };



                var hus = new Hus
                {
                    Addresse = "Osloveien18",
                    areal = 200,
                    Beskrivelse = "bla bla bla",
                    by = "Oslo",
                    erTilgjengelig = true,
                    Pris = 400,
                    romAntall = 4,
                    erMoblert = true,
                    harParkering = false,
                    bildeURL = "~/Bilder/1.jpg",
                    eier = eier
                };

                context.Add(hus);
                context.SaveChanges();

                // Now that Hus is saved and has an ID, we can update the Ordre entities with the foreign key for Hus.
                foreach (var o in ordre)
                {
                    o.husId = hus.husId;  // Assigning the foreign key for Hus
                }

                context.AddRange(ordre);
                context.SaveChanges();
            }



        }
    }
}


