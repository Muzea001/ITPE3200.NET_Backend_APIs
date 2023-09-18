using Oblig1.Migrations;
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


            if (!context.hus.Any())


            { 
                        
                var hus = new List<Hus>
                {
                    new Hus {Beskrivelse="Stort og Nyoppusset leilighet", Addresse="Osloveien22", areal=200, romAntall=4, erTilgjengelig=false},
                    

                };
                context.AddRange(hus);
                context.SaveChanges();

            }

            if (!context.kunde.Any())
            {

                var kunder = new List<Kunde>
                {
                  
                    new Kunde { Navn = "Hans", TelefonNmr = 20568799 , Addresse ="Osloveien 81", Fodselsdato = new DateTime(1990,01,01),  Email = "Hans0101@Hotmail.com"  }
                    };
                context.AddRange(kunder);
                context.SaveChanges();
            }

            if (!context.ordre.Any())
            {

                var ordre = new List<Ordre>
                {
                    new Ordre { hus = Dato= new DateTime(2023,11,1) , betaltGjennom="DelBetaling" },
                    
                    };
                context.AddRange(ordre);
                context.SaveChanges();
            }
        

            if (!context.bruker.Any())
            {

                var brukere = new List<Bruker>
                {
                    new Bruker { }
                   
                    };
                context.AddRange(brukere);
                context.SaveChanges();
            }
}
    }
}


