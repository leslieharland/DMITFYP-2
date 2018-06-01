using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using WebApp.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebApp.Formatters
{
    public class ExcelFormatter
    {
        private readonly IQueryable<StudentContact> _records;

        public ExcelFormatter(IQueryable<StudentContact> records)
        {
            _records = records;
        }

        public byte[] CreateXmlWorksheet()
        {
            var xmlTemplate = Resource.ExcelXmlTemplate;
            var styles = GetStyles();
            var header = WriteHeader();
            var xmlData = GetRecords();

            var excelXml = string.Format("{0}{1}", header, xmlData);
            xmlTemplate = xmlTemplate.Replace("$ROWSPLACEHOLDER$", excelXml);
            xmlTemplate = xmlTemplate.Replace("$STYLEPLACEHOLDER$", styles);

            return Encoding.UTF8.GetBytes(xmlTemplate);
        }

        private string GetStyles()
        {
            return @"<Styles><Style ss:ID='s1'><NumberFormat ss:Format='dd/mm/yyyy\ hh:mm:ss' />"+
                "</Style></Styles>";

        }

        private string GetRecords()
        {
            var sb = new StringBuilder();
            foreach (var record in _records)
            {
                sb.Append("<Row ss:AutoFitHeight='0'>");
                sb.Append("<Cell><Data ss:Type='String'>" + record.Id + "</Data></Cell>");
                sb.Append("<Cell><Data ss:Type='String'>" + record.FullName + "</Data></Cell>");
                sb.Append("<Cell><Data ss:Type='String'>" + record.Admin + "</Data></Cell>");
                sb.Append("<Cell><Data ss:Type='String'>" + record.Email + "</Data></Cell>");
                sb.Append("<Cell><Data ss:Type='String'>" + record.Mobile + "</Data></Cell>");
                sb.Append("</Row>"); 
            }

            return sb.ToString();
        }

        private string WriteHeader()
        {
            var header = new StringBuilder();
            header.Append("<Row ss:AutoFitHeight='0'>");
            header.Append("<Cell><Data ss:Type='String'>Id</Data></Cell>");
            header.Append("<Cell><Data ss:Type='String'>Full Name</Data></Cell>");
            header.Append("<Cell><Data ss:Type='String'>Admin</Data></Cell>");
            header.Append("<Cell><Data ss:Type='String'>Email</Data></Cell>");
            header.Append("<Cell><Data ss:Type='String'>Mobile</Data></Cell>");
            header.Append("</Row>"); 

            return header.ToString();
        }

        
        public static SpreadsheetDocument CreateExcelSpreadsheet(ref MemoryStream memoryStream)
        {
            // Passing the MemoryStream object by reference to prevent memory degradation.
            return SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook);
        }


        public static void InitializeSpreadsheet(ref SpreadsheetDocument spreadsheetDocument, Stylesheet stylesheet)
        {
            WorkbookPart workbookPart;
            WorksheetPart worksheetPart;
            WorkbookStylesPart workbookStylesPart;

            workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            if (stylesheet != null)
            {
                workbookStylesPart = spreadsheetDocument.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                workbookStylesPart.Stylesheet = stylesheet;
                workbookStylesPart.Stylesheet.Save();
            }

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "AY" + DateTime.Now.Year.ToString().Substring(2) + (DateTime.Now.Year + 1).ToString().Substring(2) + " Sem" + (System.DateTime.Now.Month >= 2 && System.DateTime.Now.Month <= 8 ? 1 : 2).ToString(),
            };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();
        }
        
        public static void FinalizeSpreadsheetWriting(ref SpreadsheetDocument spreadsheetDocument, ref SheetData sheetData)
        {
            Worksheet worksheet = new Worksheet();
            worksheet.Append(sheetData);
            spreadsheetDocument.WorkbookPart.WorksheetParts.First().Worksheet = worksheet;
            spreadsheetDocument.WorkbookPart.Workbook.Save();
            spreadsheetDocument.Close();
        }

        public static Stylesheet CreateStyles(List<Font> font, List<ForegroundColor> foregroundColor, List<BackgroundColor> backgroundColor, List<Alignment> alignment)
        {
            List<int> listSizes = new List<int>();
            listSizes.Add(font.Count);
            listSizes.Add(foregroundColor.Count);
            listSizes.Add(backgroundColor.Count);
            listSizes.Add(alignment.Count);

            int a = listSizes[0];
            for (int i = 1; i < listSizes.Count; ++i)
            {
                if (listSizes[i] != a)
                {
                    throw new ArgumentException("Invalid actual arguments passed to the CreateStyles() method.");
                }
            }

            Stylesheet stylesheet = new Stylesheet();
            stylesheet.Fonts = new Fonts();
            stylesheet.Fonts.AppendChild(new Font());
            foreach (Font f in font)
            {
                stylesheet.Fonts.AppendChild(f);
            }
            stylesheet.Fills = new Fills();
            stylesheet.Fills.Count = 0;

            for (int i = 0; i < foregroundColor.Count; ++i)
            {
                stylesheet.Fills.AppendChild(new Fill() { PatternFill = new PatternFill { PatternType = PatternValues.None } });
                stylesheet.Fills.AppendChild(new Fill() { PatternFill = new PatternFill { PatternType = PatternValues.Gray125 } });
                stylesheet.Fills.Count += 2;
                var fillColor = new PatternFill() { PatternType = PatternValues.Solid };
                fillColor.ForegroundColor = foregroundColor[i];
                fillColor.BackgroundColor = backgroundColor[i];
                stylesheet.Fills.AppendChild(new Fill() { PatternFill = fillColor });
                ++stylesheet.Fills.Count;
            }

            stylesheet.Borders = new Borders();
            stylesheet.Borders.Count = 1;
            stylesheet.Borders.AppendChild(new Border());

            stylesheet.CellStyleFormats = new CellStyleFormats();
            stylesheet.CellStyleFormats.Count = 1;
            stylesheet.CellStyleFormats.AppendChild(new CellFormat());

            stylesheet.CellFormats = new CellFormats();
            stylesheet.CellFormats.AppendChild(new CellFormat());
            stylesheet.CellFormats.Count = 1;

            for (int i = 0; i < font.Count; ++i)
            {
                stylesheet.CellFormats.AppendChild(new CellFormat { FormatId = 0, FontId = (uint)(i + 1), BorderId = 0, FillId = (uint)(i + 2), ApplyFill = true, Alignment = alignment[i] });
                ++stylesheet.CellFormats.Count;
            }
            return stylesheet;
        }

        public static Row CreateRow(string startingCellReference, uint rowIndex, ICollection<string> rowData, uint styleIndex)
        {
            Row row = new Row() { RowIndex = rowIndex };
            foreach (string s in rowData)
            {
                if (s == null)
                {
                    row.Append(new Cell()
                    {
                        CellReference = startingCellReference,
                        DataType = CellValues.String,
                        CellValue = new CellValue("NULL"),
                        StyleIndex = styleIndex
                    });
                }
                else if (!new Regex("^[0-9]+$").IsMatch(s))
                {
                    row.Append(new Cell()
                    {
                        CellReference = startingCellReference,
                        DataType = CellValues.String,
                        CellValue = new CellValue(s),
                        StyleIndex = styleIndex
                    });
                }
                else
                {
                    row.Append(new Cell()
                    {
                        CellReference = startingCellReference,
                        DataType = CellValues.Number,
                        CellValue = new CellValue(s),
                        StyleIndex = styleIndex
                    });
                }
                startingCellReference = ((char)(startingCellReference[0] + 1)).ToString() + startingCellReference.Substring(1);
            }
            return row;
        }
    }
}