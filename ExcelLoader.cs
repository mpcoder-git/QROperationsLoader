using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Excel = Microsoft.Office.Interop.Excel;

namespace QROperationsLoader
{
    internal class ExcelLoader
    {

        public string[,] LoadExcelFile(string filePath)
        {

            /*
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(filePath);
            Excel.Worksheet excelWorksheet = excelWorkbook.Sheets[1];
            Excel.Range excelRange = excelWorksheet.UsedRange;

            int rowCount = excelRange.Rows.Count;
            int colCount = excelRange.Columns.Count;

            // Выбор диапазона ячеек по номерам строк и столбцов
            Excel.Range startCell = excelWorksheet.Cells[1, 1]; // начальная ячейка диапазона A1
            Excel.Range endCell = excelWorksheet.Cells[rowCount, colCount]; // конечная ячейка диапазона 


            int iLastRow = excelWorksheet.Cells[excelWorksheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;  //последняя заполненная строка в столбце А
            var arrData = (object[,])excelWorksheet.Range[startCell, endCell].Value;

            return arrData;
            */

                var package = new ExcelPackage(new FileInfo(filePath));
            
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; // Assuming data is on the first worksheet
                var cells = worksheet.Cells;

                // Get the dimension of the data
                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;

                // Create a string array to store the data
                string[,] arrData = new string[rows, columns];

                for (int row = 1; row <= rows; row++)
                {
                    for (int col = 1; col <= columns; col++)
                    {
                        // Convert cell value to string and store in the array
                        arrData[row - 1, col - 1] = cells[row, col].Value?.ToString();
                    }
                }
                //Console.Read();
                // Use the arrData string array as needed
            

                return arrData;

        }



    }
}
