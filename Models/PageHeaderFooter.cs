using iTextSharp.text.pdf;
using iTextSharp.text;

namespace partner_aluro.Models
{
    public class PageHeaderFooter : PdfPageEventHelper
    {
        private readonly Font _pageNumberFont = new Font(Font.FontFamily.HELVETICA, 8f, Font.NORMAL);



        private readonly Font regular = new Font(BaseFont.CreateFont(@"wwwroot\css\font\arial.ttf", BaseFont.CP1250, true), 10);

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            AddPageNumber(writer, document);
        }

        private void AddPageNumber(PdfWriter writer, Document document)
        {
            var text1 = "ALURO fashion at home - ul. Gnieźnieńska 161 62-006 Kobylnica Polska";
            var text = "Strona: " + writer.PageNumber.ToString() + "/" + writer.PageNumber.ToString();

            var numberTable = new PdfPTable(1);
            var numberCell = new PdfPCell(new Phrase(text, _pageNumberFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
            numberCell.Border = Rectangle.TOP_BORDER;
            numberCell.BorderWidth = 0;
            numberCell.PaddingTop = 1;
            numberCell.PaddingBottom = 1;


            numberTable.AddCell(numberCell);
            numberTable.TotalWidth = 50;
            numberTable.WriteSelectedRows(0, -1, document.Right - 80, document.Bottom + 20, writer.DirectContent);

            var numberTable2 = new PdfPTable(1);
            var numberCell2 = new PdfPCell(new Phrase(text1, regular)) { HorizontalAlignment = Element.ALIGN_CENTER };

            numberCell2.Border = Rectangle.TOP_BORDER;
            numberCell2.BorderWidth = 1;
            numberCell2.PaddingTop = 5;
            numberCell2.PaddingBottom =5;

            numberTable2.AddCell(numberCell2);

            numberTable2.TotalWidth = 500;
            numberTable2.WriteSelectedRows(0, -1, document.Left+21, document.Bottom + 5, writer.DirectContent);

        }

    }
}
