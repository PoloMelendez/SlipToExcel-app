using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace SlipToExcel
{
    //Accesses an existing excel workbook, creates a new worksheet, and adds values to it
    class ExcelBuilder
    {
        private string WorkbookPath = null;
        private static Excel.Application ExcelApp = null;
        private static Excel.Workbook Workbook = null;
        private static Excel.Worksheet Worksheet = null;

        public ExcelBuilder() { ExcelApp = new Excel.Application(); }
        public ExcelBuilder(string path)
        {
            ExcelApp = new Excel.Application();
            WorkbookPath = path;
            Workbook = ExcelApp.Workbooks.Open(WorkbookPath);
        }

        public void Close()
        {
            Workbook.RefreshAll();
            ExcelApp.Calculate();
            Workbook.Save();
            Workbook.Close();
            Marshal.ReleaseComObject(Worksheet);
            Marshal.ReleaseComObject(Workbook);
            ExcelApp.Quit();
        }

        public Excel.Worksheet AddWorksheet(string worksheetName, string templatePath = "")
        {
            if (Workbook != null)
            {
                Worksheet = Workbook.Sheets.Add(After: Workbook.Sheets[Workbook.Sheets.Count],
                                                                            Type: templatePath);
                while (true)
                {
                    try
                    {
                        Worksheet.Name = worksheetName;
                        return Worksheet;
                    }
                    catch (Exception ex)
                    {
                        worksheetName += "(copy)";
                    }
                }
            }
            return new Excel.Worksheet();
        }

        //Accepts the string list created from the packing slips, using SlipParser, 
        //and populates a worksheets cells
        public void Convert(List<string[]> list)
        {
            int row = 3;
            foreach (string[] arr in list)
            {
                for (int i = 0; i < Int32.Parse(arr[0]); i++)
                {
                    Worksheet.Cells[row, 1] = arr[1];
                    Worksheet.Cells[row, 3] = arr[2];
                    row++;
                }
            }
            Worksheet.Columns[1].AutoFit();
        }
    }
}
