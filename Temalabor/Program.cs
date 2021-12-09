using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Linq;
using System.Text;

namespace Temalabor
{
    class Program
    {
        static void Main(string[] args)
        {

            StringBuilder sb = new StringBuilder(); //Ebbe kerul bele a kiolvasott adat
            string file = @"D:\Egyetem\5.felev\Temalabor\Csenge_szenelyi.pdf"; //a pdf amiből olvasódik ki az adat

            ///Az IText -el igy lehet adatot kiolvasni egy pdfbol
            using (PdfReader reader = new PdfReader(file))
            {
                PdfDocument pdfdoc = new PdfDocument(reader);

                for (int pageNo = 1; pageNo <= pdfdoc.GetNumberOfPages(); pageNo++)
                {
                    ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                    string text = PdfTextExtractor.GetTextFromPage(pdfdoc.GetPage(pageNo), strategy);
                    sb.Append(text);

                }
            }

            ///A DataExtractor oszatly megkapja a StringBuildert, amiben a kiolvasott stringek vannak
            DataExtractor dataExtractor = new DataExtractor(sb);

            ///Kibanyassza az adatokat
            dataExtractor.extract();

            Console.ReadKey();
        }
    }
}
