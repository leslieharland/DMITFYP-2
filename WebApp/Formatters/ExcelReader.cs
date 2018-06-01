using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.DAL;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Text.RegularExpressions;

namespace WebApp.Formatters
{
    public class ExcelReader
    {
        private static string getCellValue(ref SpreadsheetDocument spreadsheetDocument, Cell cell)
        {
            return spreadsheetDocument.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault().SharedStringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;
        }
        public static object readStudentData(Stream excelSpreadsheetFile)
        {
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(excelSpreadsheetFile, false);

            // An excel document typically contains a workbook which contains many worksheets.

            /*
             * This line of code below thus attempts to access the workbook first, from the excel document,
             * hence the code (excerpt); spreadsheetDocument.WorkbookPart. Next after the acquisition of the
             * workbook, are the worksheets within it. Particularly, of interest, is the first worksheet,
             * hence the code excerpt; WorksheetParts.First().
             */
            WorksheetPart worksheetPart = spreadsheetDocument.WorkbookPart.WorksheetParts.First();
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            int iterationCount = 0;
            string cellValue = "";
            List<string> reasonsForInvalidity = new List<string>();
            List<Student> listOfValidStudents = new List<Student>();
            Queue<string> listOfRowNumbersForValidStudents = new Queue<string>();
            Queue<string> listOfRowNumbersForInvalidStudents = new Queue<string>();
            bool studentValid = false;
            string rowNumber = "";
            string invalidityReason = "";

            Student s;

            foreach (Row r in sheetData.Elements<Row>())
            {
                if (r.RowIndex == 1) continue; // ignore the first row as it contains the headers

                // validating the integrity of the spreadsheet, starting of with column number validation
                if (r.Elements<Cell>().Count() != 5) throw new Exception("Too few/ many columns detected in a row, in the uploaded spreadsheet.");
                s = new Student();
                iterationCount = 0;
                foreach (Cell c in r.Elements<Cell>())
                {
                    ++iterationCount;
                    if (c.DataType != null)
                    {
                        // another validation of the spreadsheet's integrity, validating the datatype of the cell
                        if (c.DataType != "s") throw new Exception("Cell with invalid value detected, in the uploaded spreadsheet.");
                        if (!(iterationCount == 2 || iterationCount == 3 || iterationCount == 5)) throw new Exception("Cell with invalid value detected, in the uploaded spreadsheet.");
                        // to avoid performance degradation the SpreadsheetDocument should not be passed to the getCellValue by value, it should instead be passed by reference
                        // the ref keyword is used to pass a method parameter by reference
                        cellValue = getCellValue(ref spreadsheetDocument, c);
                        switch (iterationCount)
                        {
                            case 2:
                                studentValid = new Regex("^p[0-9]{7}$").IsMatch(cellValue);
                                if (studentValid == false)
                                    invalidityReason = "Invalid admission number detected, in the uploaded spreadsheet.";
                                s.admin_number = cellValue.Substring(1);
                                break;
                            case 3:
                                studentValid = new Regex("^([A-Z]{1}[a-z]+){1}( [A-Z]{1}[a-z]+)*$").IsMatch(cellValue);
                                if (studentValid == false)
                                    invalidityReason = "Invalid full name detected, in the uploaded spreadsheet.";
                                s.full_name = cellValue;
                                break;
                            case 5:
                                studentValid = new Regex("^[A-Za-z0-9\\.]+@([A-Za-z]+)(\\.[A-Za-z]+)*$").IsMatch(cellValue);
                                if (studentValid == false)
                                {
                                    invalidityReason = "Invalid email address detected, in the uploaded spreadsheet.";
                                }
                                s.email_address = cellValue;
                                break;
                        }
                        if (!studentValid) break;
                    }
                    else
                    {
                        cellValue = c.InnerText;
                        if (!(iterationCount == 1 || iterationCount == 4)) throw new Exception("Cell with invalid value detected, in the uploaded spreadsheet.");
                        switch (iterationCount)
                        {
                            case 1:
                                if (!(new Regex("^[0-9]+$").IsMatch(cellValue))) throw new Exception("Invalid serial number detected, in the uploaded spreadsheet.");
                                studentValid = true;
                                rowNumber = cellValue;
                                break;
                            case 4:
                                studentValid = new Regex("^[8-9]{1}[0-9]{7}$").IsMatch(cellValue);
                                if (studentValid == false)
                                {
                                    invalidityReason = "Invalid mobile number detected, in the uploaded spreadsheet.";
                                }
                                s.mobile_number = cellValue;
                                break;
                        }
                        if (!studentValid) break;
                    }
                }

                if (studentValid == true)
                {
                    listOfRowNumbersForValidStudents.Enqueue(rowNumber);
                    listOfValidStudents.Add(s);
                }
                else
                {
                    listOfRowNumbersForInvalidStudents.Enqueue(rowNumber);
                    reasonsForInvalidity.Add(invalidityReason);
                }
            }
            return new { validStudents = listOfValidStudents, validRowNumbers = listOfRowNumbersForValidStudents, invalidRowNumbers = listOfRowNumbersForInvalidStudents, invalidityReasons = reasonsForInvalidity };
        }
    }
}