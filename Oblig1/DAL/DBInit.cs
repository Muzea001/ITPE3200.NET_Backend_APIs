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

            if (!context.hus.Any())
            {
                var hus = new List<Hus>
                {
                    new Hus {Beskrivelse="Stort og Nyoppusset leilighet", addresse="Osloveien22", areal=200, romAntall=4, erTilgjengelig=false},
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
                    new Kunde { Id=1, navn = "Hans", telefonNmr = 20568799 , addresse ="Osloveien 81", fodselsdato = new DateTime(1990,01,01),  email = "Hans0101@Hotmail.com"  }
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
        }
    }
}


