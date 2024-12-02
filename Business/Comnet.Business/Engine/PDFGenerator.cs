using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Comnet.Data.Contracts.ViewModels.Assessment;


namespace Comnet.Business.Engine
{
    public class PDFGenerator : IPDFGenerator
    {
        public byte[] GeneratePdf(AssessmentReportDetails details)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Initialize PDF writer
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4);
                //int totalPages = 10; // Example total pages

                string title = "Skill Matrix";  // Replace with the actual logo or text
                string subTitle = $"{details.AssessmentBy} - {details.UserName}";

                // Add event handler for header and page number
                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new MarginalsEventHandler(document, title, subTitle));

                document.SetMargins(70, 36, 36, 36);  // Increase the first value for more space

                document.Add(new Paragraph($"{details.UserName}\n").SetFontSize(18));

                #region Assessment Details
                Table assessment = new(2);
                assessment.SetWidth(UnitValue.CreatePercentValue(70));

                #region CompletedBy                
                assessment.AddCell(new Cell().Add(new Paragraph("Completed by").SetBold()));
                assessment.AddCell(new Cell().Add(new Paragraph(details.AssessmentByUser)));
                #endregion

                #region CompletionDate
                assessment.AddCell(new Cell().Add(new Paragraph("Completion date").SetBold()));
                assessment.AddCell(new Cell().Add(new Paragraph(details.Date)));
                #endregion

                #region SkillSet
                assessment.AddCell(new Cell().Add(new Paragraph("Skill set").SetBold()));
                assessment.AddCell(new Cell().Add(new Paragraph(details.SkillSet)));
                #endregion

                #region OverallComment
                assessment.AddCell(new Cell(0, 2).SetBorder(Border.NO_BORDER).Add(new Paragraph("Overall comments").SetBold()));
                assessment.AddCell(new Cell(0, 2).SetBorder(Border.NO_BORDER).Add(new Paragraph(details.Comment)));
                #endregion

                document.Add(assessment);
                document.Add(new Paragraph("\n\n\n"));
                #endregion


                foreach (var category in details.Categories)
                {
                    Table table = new Table(4); // 4 columns
                    table.SetWidth(UnitValue.CreatePercentValue(100));

                    table.AddCell(new Cell().Add(new Paragraph(category.CategoryName).SetBold().SetFontSize(14)).SetPadding(5).SetBorder(Border.NO_BORDER).SetWidth(UnitValue.CreatePercentValue(45)));
                    table.AddCell(new Cell().Add(new Paragraph("Skill Level")).SetPadding(5).SetTextAlignment(TextAlignment.CENTER).SetWidth(UnitValue.CreatePercentValue(10)));
                    if ("Supervisor Assessment" == details.AssessmentBy)
                    {
                        table.AddCell(new Cell().Add(new Paragraph("")).SetPadding(5).SetBorder(Border.NO_BORDER).SetWidth(UnitValue.CreatePercentValue(10)));
                    }
                    else
                    {
                        table.AddCell(new Cell().Add(new Paragraph("Interest")).SetPadding(5).SetTextAlignment(TextAlignment.CENTER).SetWidth(UnitValue.CreatePercentValue(10)));
                    }
                    table.AddCell(new Cell().Add(new Paragraph("Comments")).SetPadding(5).SetBorder(Border.NO_BORDER).SetWidth(UnitValue.CreatePercentValue(35)));

                    foreach (var skill in category.Skills)
                    {
                        table.AddCell(new Cell()
                            .Add(new Paragraph(skill.SkillName).SetBold())
                            .Add(new Paragraph(skill.SkillDescription ?? ""))
                        );

                        table.AddCell(new Cell().Add(new Paragraph(skill.SkillLevel.ToString()).SetPadding(5).SetTextAlignment(TextAlignment.CENTER)));

                        if ("Supervisor Assessment" == details.AssessmentBy)
                        {
                            table.AddCell(new Cell().Add(new Paragraph("")).SetPadding(5).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                        }
                        else
                        {
                            table.AddCell(new Cell().Add(new Paragraph(skill.InterestLevel.ToString())).SetPadding(5).SetTextAlignment(TextAlignment.CENTER));
                        }

                        if (skill == category.Skills.First())
                        {
                            table.AddCell(new Cell(category.Skills.Count, 0).Add(new Paragraph(category.Comment)).SetPadding(5).SetBorder(Border.NO_BORDER));
                        }
                    }
                    document.Add(table);

                    if(category != details.Categories.Last()) document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }

                document.Close();

                return memoryStream.ToArray();
            }
        }


    }

    public class MarginalsEventHandler : IEventHandler
    {
        private Document _document;
        private string _title;
        private string _subTitle;
        public MarginalsEventHandler(Document document, string title, string subTitle)
        {
            _document = document;
            _title = title;
            _subTitle = subTitle;
        }

        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent pdfDocEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = pdfDocEvent.GetDocument();
            Rectangle pageSize = pdfDocEvent.GetPage().GetPageSize();
            PdfCanvas pdfCanvas = new PdfCanvas(pdfDocEvent.GetPage());
            Canvas canvas = new Canvas(pdfCanvas, pageSize);

            // Add header content
            #region Header
            Table headerTable = new Table(2);
            headerTable.SetWidth(UnitValue.CreatePercentValue(100));

            Paragraph title = new Paragraph(_title)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(16)
                .SetBold();

            ;
            headerTable.AddHeaderCell(new Cell().Add(title).SetBorder(Border.NO_BORDER));
            headerTable.AddHeaderCell(new Cell().Add(new Paragraph(_subTitle).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(Border.NO_BORDER));
            headerTable.AddCell(new Cell(0, 2).Add(new LineSeparator(new SolidLine(0.5f))).SetBorder(Border.NO_BORDER));

            canvas.ShowTextAligned(new Paragraph(), 0, pageSize.GetTop(), TextAlignment.CENTER);  // Placeholder to move to the top
            headerTable.SetFixedPosition(30, pageSize.GetTop() - 50, pageSize.GetWidth() - 60);
            canvas.Add(headerTable);
            #endregion

            #region Footer
            int pageNumber = pdfDoc.GetPageNumber(pdfDocEvent.GetPage());
            int totalPages = pdfDoc.GetNumberOfPages();
            canvas.ShowTextAligned(
                new Paragraph("Page " + pageNumber), //+ " of " + totalPages),
                pageSize.GetWidth() / 2,
                20,
                TextAlignment.CENTER
            );  //Will change event
            #endregion

            //_document.SetMargins(100, 36, 36, 36);
            canvas.Close();
        }

        //private void AddHeader(Canvas canvas, Rectangle pageSize, string title, string subTitle)
        //{
        //    // Create a table with two columns
        //    Table headerTable = new Table(2);
        //    headerTable.SetWidth(UnitValue.CreatePercentValue(100));

        //    // Add SkillsBase logo on the left (use a real logo if available)
        //    Cell logoCell = new Cell().Add(new Paragraph(title));
        //    logoCell.SetBorder(Border.NO_BORDER);
        //    headerTable.AddCell(logoCell);

        //    // Add "Supervisor Assessment - Rohit Soni" on the right
        //    Cell textCell = new Cell().Add(new Paragraph(subTitle)
        //                                   .SetTextAlignment(TextAlignment.RIGHT));
        //    textCell.SetBorder(Border.NO_BORDER);
        //    headerTable.AddCell(textCell);

        //    // Position the table at the top of the page
        //    canvas.ShowTextAligned(new Paragraph(), 0, pageSize.GetTop(), TextAlignment.CENTER);  // Placeholder to move to the top
        //    headerTable.SetFixedPosition(0, pageSize.GetTop() - 40, pageSize.GetWidth());
        //    canvas.Add(headerTable);
        //}
    }
}