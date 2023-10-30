

using Microsoft.AspNetCore.Identity;
using Oblig1.Models;
using System;
using System.Drawing.Text;

namespace Oblig1.DAL
{
    public class DBInit
    {

        

        

        public static async Task Seed(IApplicationBuilder app )
        {

        

            using var serviceScope = app.ApplicationServices.CreateScope();
            ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            using var scope = app.ApplicationServices.CreateScope();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Person>>();
            context.Database.EnsureCreated();




            var roles = new[] { "Admin", "Bruker" };

            foreach ( var role in roles ) 
                {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            }
            string navn = "Admin";
            int telefonnummer = 40336208;
            string adresse = "Tokerudbekken 16";
            DateTime fodselsdato = DateTime.Now;
            string email = "Admin@gmail.com";
            string passord = "Admin123!";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                
                var user = new Person();
                user.UserName = email;
                user.Email = email;
                user.EmailConfirmed = true;
                user.Addresse = adresse;
                user.TelefonNmr = telefonnummer;    
                user.Navn= navn;
                user.Fodselsdato= fodselsdato;
               

               await userManager.CreateAsync(user,passord);
               await userManager.AddToRoleAsync(user, "Admin");
            }
            string defaultPass = "Hei-12345";

            if (!context.Kunde.Any() && !context.Eier.Any())


            {
                
                var person = new Person
                {

                    Navn="Mathias",Addresse="Osloveien18",Email="Mathias18@gmail.com",Fodselsdato= new DateTime(1999,5,8),TelefonNmr=14789531, UserName= "Mathias18@gmail.com"

                };

                await userManager.CreateAsync(person, defaultPass);

                var kunde = new Kunde
                {
                    Person = person,
                    husListe = new List<Hus>(),
                    ordreListe = new List<Ordre>()
                   
               
                };
                var eier = new Eier
                {
                    Person = person,
                    kontoNummer = 11111111111,
                    antallAnnonser = 0,
                    husListe = new List<Hus>()
                    
                };

 

                
                context.AddRange(kunde,eier);
                context.SaveChanges();
            }

            


            if (!context.Hus.Any())

            {

                var person = new Person
                {

                    Navn = "Ole",
                    Addresse = "Osloveien22",
                    Email = "Jonas13@gmail.com",
                    Fodselsdato = new DateTime(1985, 7, 4),
                    TelefonNmr = 87546611,
                    UserName = "Jonas13@gmail.com",
                    
                    

                };
                await userManager.CreateAsync(person, defaultPass);

                var eier = new Eier
                {
                    Person = person,
                    kontoNummer = 22222222222,
                    antallAnnonser = 0,
                    
                };


                var bildeListe1 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic1.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic2.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic3.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic4.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic5.jpg"}



                };

                var bildeListe2 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic2.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic3.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic4.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic5.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic1.jpg"}



                };
                var bildeListe3 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic3.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic4.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic5.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic1.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic2.jpg"}



                };

                var bildeListe4 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic4.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic5.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic1.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic2.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic3.jpg"}



                };

                var bildeListe5 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic5.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic1.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic2.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic3.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic4.jpg"}



                };

                var bildeListe6 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic7.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic1.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic2.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic3.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic4.jpg"}



                };
                var bildeListe7 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic6.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic7.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic8.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic9.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic10.jpg"}



                };

                var bildeListe8 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic7.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic8.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic9.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic10.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic1.jpg"}



                };

                var bildeListe9 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic8.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic9.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic10.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic1.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic2.jpg"}



                };

                var bildeListe10 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic10.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic9.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic8.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic7.jpg", },
                new Bilder {bilderUrl="/Bilder/Pic6.jpg"}



                };







                var hus = new List<Hus>
                {
                    new Hus {Addresse="Osloveien18",areal=200,Beskrivelse="bla bla bla",
                        by="Oslo",Pris=400,romAntall=4, erMoblert=true, harParkering=false,bildeListe=bildeListe1, eier = eier },
                    new Hus {Addresse="Osloveien22", areal=250, Beskrivelse="bla bla ",
                        by="Oslo",Pris=200,romAntall=3,erMoblert=false,harParkering=true,bildeListe=bildeListe2, eier = eier },
                    new Hus {Addresse="Trondheimsgate 10", areal=220, Beskrivelse="Spacious house with a view of the river.",
                        by="Trondheim", Pris=750, romAntall=6, erMoblert=false, harParkering=true,bildeListe=bildeListe3, eier = eier },
                    new Hus {Addresse="Stavangerveien 5", areal=160, Beskrivelse="Modern townhouse in a quiet neighborhood.",
                        by="Stavanger", Pris=500, romAntall=4, erMoblert=true, harParkering=false,bildeListe=bildeListe4, eier = eier},
                    new Hus {Addresse="Bergensgate 22", areal=180, Beskrivelse="A cozy family home with a garden.", by="Bergen",
                        Pris=600, romAntall=5, erMoblert=true, harParkering=true,bildeListe=bildeListe5, eier = eier},
                    new Hus {Addresse="Bodogate 8", areal=190, Beskrivelse="Rustic cottage in the countryside.",
                        by="Bodo", Pris=450, romAntall=3, erMoblert=true, harParkering=false,bildeListe=bildeListe6, eier = eier},
                    new Hus {Addresse="Tromsogate 15", areal=240, Beskrivelse="Spacious family home with a view of the mountains.",
                        by="Tromso", Pris=950, romAntall=8, erMoblert=true, harParkering=true,bildeListe=bildeListe7, eier = eier},
                    new Hus {Addresse="Bergengate 30", areal=170, Beskrivelse="Cozy apartment in the city center.",
                        by="Bergen", Pris=700, romAntall=4, erMoblert=true, harParkering=false,bildeListe=bildeListe8, eier = eier},
                    new Hus {Addresse="Kristiansandveien 7", areal=200, Beskrivelse="Modern duplex with a rooftop terrace.",
                        by="Kristiansand", Pris=800, romAntall=6, erMoblert=false, harParkering=true,bildeListe=bildeListe9, eier = eier },
                    new Hus {Addresse="Alesundgate 25", areal=185, Beskrivelse="Dette er ",
                        by="Alesund", Pris=550, romAntall=5, erMoblert=false, harParkering=true, bildeListe=bildeListe10, eier = eier},



            };

                
              


                context.AddRange(hus);
                context.SaveChanges();

            }



            if (!context.Ordre.Any())
            {
                var person = new Person
                {
                    Navn = "Marius",
                    Addresse = "Hammerveien22",
                    Email = "Marius22@gmail.com",
                    Fodselsdato = new DateTime(2001, 4, 3),
                    TelefonNmr = 14789531,
                    UserName = "Marius22@gmail.com",
                    
                };
                await userManager.CreateAsync(person, defaultPass);



                var kunde = new Kunde
                {
                    Person = person,
                    ordreListe = new List<Ordre>(),
                    husListe = new List<Hus>(),
                    
                };

                context.Add(kunde);
                context.SaveChanges();

               

                var eier = new Eier
                {
                    Person = person,
                    kontoNummer = 33333333333,
                    antallAnnonser = 0
                };

                var bildeListe11 = new List<Bilder>
                {
                new Bilder {bilderUrl= "/Bilder/Pic1.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic2.jpg"},
                new Bilder {bilderUrl= "/Bilder/Pic3.jpg"},
                new Bilder {bilderUrl="/Bilder/Pic4.jpg" },
                new Bilder {bilderUrl="/Bilder/Pic5.jpg"}



                };

                var hus = new Hus
                {
                    Addresse = "Osloveien18",
                    areal = 200,
                    Beskrivelse = "Dette er et hus som får inn bilder",
                    by = "Oslo",
                    Pris = 400,
                    romAntall = 4,
                    bildeListe= bildeListe11,
                    erMoblert = true,
                    harParkering = false,
                    eier = eier
                };

                kunde.husListe.Add(hus);
                context.Add(hus);
                context.SaveChanges();
                var ordre = new List<Ordre>
                  {
                   new Ordre
                 {

                   betaltGjennom = "Kort",
                   startDato = new DateTime(2023,5,8),
                  sluttDato = new DateTime(2023,7,8),
                  fullPris = 8700,
                    kunde = kunde,
                    hus = hus

                      },
                    new Ordre
                     {

                   betaltGjennom = "Klarna",
                   startDato = new DateTime(2023,1,1),
                  sluttDato = new DateTime(2023,1,15),
                  fullPris = 12000,
                   kunde=kunde,
                   hus = hus

                         }
                     };
                kunde.ordreListe = ordre;

                
               
                foreach (var o in ordre)
                {
                    o.hus.husId = hus.husId;
                    o.kunde.kundeID = kunde.kundeID;
                }

                context.AddRange(ordre);
                context.SaveChanges();
            }



        }
    }
}


