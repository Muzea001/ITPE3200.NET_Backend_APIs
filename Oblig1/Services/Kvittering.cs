using DinkToPdf.Contracts;
using DinkToPdf;

namespace Oblig1.Services
{
    public class Kvittering
    {
        private readonly IConverter _converter;

        public Kvittering(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] genererPdfKvittering(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
                Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                }
            }
            };

            return _converter.Convert(doc);
        }
        public bool erVelykket { get; set; }
        public string feilMelding { get; set; }


    }
}
