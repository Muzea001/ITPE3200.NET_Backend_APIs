using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Oblig1.Models;

namespace Oblig1.Controllers
{
    public class HusOgBilderViewModell
    {
      public Hus hus { get; set; }
      public List<IFormFile> bilder { get; set; }
    }
}
