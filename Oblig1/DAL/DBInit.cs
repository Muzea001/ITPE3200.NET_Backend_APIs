using Oblig1.Models;

namespace Oblig1.DAL
{
    public class DBInit
    {

        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();


            Bruker bruker = new Bruker() {Id=1,Navn="Petter",Addresse="Osloveien123",Email="Petter@hotmail.com",Fodselsdato=new DateTime(1992,02,02),TelefonNmr=50663211 };
            Eeier eier = new Eeier() {bruker = bruker, kontoNummer=193265478 };

            if (!context.hus.Any())
            {

               
                        
                var hus = new List<Hus>
                {
                    new Hus {Beskrivelse="Stort og Nyoppusset leilighet", Addresse="Osloveien22", areal=200, romAntall=4, erTilgjengelig=false, Eeier=eier},
                    new Hus { },
                    new Hus { },
                    new Hus { },
                    new Hus { },
                    new Hus { },
                    new Hus { },
                    new Hus { },
                    new Hus { },
                    new Hus { }

                };
                context.AddRange(hus);
                context.SaveChanges();

            }

            if (!context.kunde.Any())
            {

                var kunder = new List<Kunde>
                {
                    new Kunde { },
                    new Kunde { },
                    new Kunde { Id=1, Navn = "Hans", TelefonNmr = 20568799 , Addresse ="Osloveien 81", Fodselsdato = new DateTime(1990,01,01),  Email = "Hans0101@Hotmail.com"  }
                    };
                context.AddRange(kunder);
                context.SaveChanges();
            }

            if (!context.ordre.Any())
            {

                var ordre = new List<Ordre>
                {
                    new Ordre {ordreId=1 ,Dato= new DateTime(2023,11,1) ,husId=1, betaltGjennom="DelBetaling" },
                    new Ordre { },
                    new Ordre { }
                    };
                context.AddRange(ordre);
                context.SaveChanges();
            }
        

            if (!context.bruker.Any())
            {

                var brukere = new List<Bruker>
                {
                    new Bruker {Id=1,Navn="Hans Eli",Addresse="Osloveien32", Email="Hans@Hotmail.com",Fodselsdato= new DateTime(1889,03,03), TelefonNmr=98256374},
                    new Bruker { },
                    new Bruker { }
                    };
                context.AddRange(brukere);
                context.SaveChanges();
            }
}
    }
}


